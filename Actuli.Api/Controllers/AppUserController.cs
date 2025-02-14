using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Actuli.Api.Models;
using Actuli.Api.Services;
using Microsoft.Azure.Cosmos;

namespace Actuli.Api.Controllers;

[Authorize]
[Route("api/user")]
[ApiController]
public class AppUserController : ControllerBase
{
    private readonly CosmosDbService _cosmosDbService;

    public AppUserController(CosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }

    /// <summary>
    /// The 'oid' (object id) is the only claim that should be used to uniquely identify
    /// a user in an external tenant. The token might have one or more of the following claim,
    /// that might seem like a unique identifier, but is not and should not be used as such:
    ///
    /// - upn (user principal name): might be unique amongst the active set of users in a tenant
    /// but tend to get reassigned to new employees as employees leave the organization and others
    /// take their place or might change to reflect a personal change like marriage.
    ///
    /// - email: might be unique amongst the active set of users in a tenant but tend to get reassigned
    /// to new employees as employees leave the organization and others take their place.
    /// </summary>
    private Guid GetUserId()
    {
        Guid userId;

        if (!Guid.TryParse(HttpContext.User.GetObjectId(), out userId))
        {
            throw new Exception("User ID is not valid.");
        }

        return userId;
    }

    /// <summary>
    /// Access tokens that have neither the 'scp' (for delegated permissions) nor
    /// 'roles' (for application permissions) claim are not to be honored.
    ///
    /// An access token issued by Azure AD will have at least one of the two claims. Access tokens
    /// issued to a user will have the 'scp' claim. Access tokens issued to an application will have
    /// the roles claim. Access tokens that contain both claims are issued only to users, where the scp
    /// claim designates the delegated permissions, while the roles claim designates the user's role.
    ///
    /// To determine whether an access token was issued to a user (i.e delegated) or an application
    /// more easily, we recommend enabling the optional claim 'idtyp'. For more information, see:
    /// https://docs.microsoft.com/azure/active-directory/develop/access-tokens#user-and-application-tokens
    /// </summary>
    private bool IsAppMakingRequest()
    {
        // Add in the optional 'idtyp' claim to check if the access token is coming from an application or user.
        // See: https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-optional-claims
        if (HttpContext.User.Claims.Any(c => c.Type == "idtyp"))
        {
            return HttpContext.User.Claims.Any(c => c.Type == "idtyp" && c.Value == "app");
        }
        else
        {
            // alternatively, if an AT contains the roles claim but no scp claim, that indicates it's an app token
            return HttpContext.User.Claims.Any(c => c.Type == "roles") && !HttpContext.User.Claims.Any(c => c.Type == "scp");
        }
    }

    // TODO - REMOVED IMPLEMENTATION FOR SIMPLICITY - NEED TO REINSTATE
    private bool RequestCanAccessApplicationUser(Guid userId)
    {
        return IsAppMakingRequest() || (userId == GetUserId());
    }

    [HttpGet]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Read",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Read"
    )]
    public async Task<IActionResult> GetAsync()
    {
        string userId = GetUserId().ToString();
        AppUser applicationUser = await _cosmosDbService.GetItemAsync<AppUser>(userId);

        if (applicationUser is null)
        {
            try
            {
                // TODO: NEED TO CREATE USER ON SIGNUP AND REMOVE THIS SECTION
                applicationUser = new AppUser(userId);
                await _cosmosDbService.AddItemAsync(applicationUser);
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Request Diagnostics: {ex.Diagnostics}");
            }
        }

        return Ok(applicationUser);
    }

    [HttpPut]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> PutAsync([FromBody] AppUser appUser)
    {
        string userId = GetUserId().ToString();

        // Load the stored user
        var storedApplicationUser = await _cosmosDbService.GetItemAsync<AppUser>(userId);

        if (storedApplicationUser is null)
        {
            return NotFound("User not found.");
        }

        // Update only non-null fields
        storedApplicationUser.Username = appUser.Username ?? storedApplicationUser.Username;
        storedApplicationUser.Name = appUser.Name ?? storedApplicationUser.Name;
        storedApplicationUser.Profile.Contact.Email = appUser.Profile.Contact.Email ?? storedApplicationUser.Profile.Contact.Email;
        storedApplicationUser.Profile.Contact.FirstName = appUser.Profile.Contact.FirstName ?? storedApplicationUser.Profile.Contact.FirstName;
        storedApplicationUser.Profile.Contact.LastName = appUser.Profile.Contact.LastName ?? storedApplicationUser.Profile.Contact.LastName;
        storedApplicationUser.Profile.Contact.Address1 = appUser.Profile.Contact.Address1 ?? storedApplicationUser.Profile.Contact.Address1;
        storedApplicationUser.Profile.Contact.Address2 = appUser.Profile.Contact.Address2 ?? storedApplicationUser.Profile.Contact.Address2;
        storedApplicationUser.Profile.Contact.City = appUser.Profile.Contact.City ?? storedApplicationUser.Profile.Contact.City;
        storedApplicationUser.Profile.Contact.State = appUser.Profile.Contact.State ?? storedApplicationUser.Profile.Contact.State;
        storedApplicationUser.Profile.Contact.PostalCode = appUser.Profile.Contact.PostalCode ?? storedApplicationUser.Profile.Contact.PostalCode;
        storedApplicationUser.Profile.Contact.Country = appUser.Profile.Contact.Country ?? storedApplicationUser.Profile.Contact.Country;
        storedApplicationUser.Profile.Contact.DateOfBirth = appUser.Profile.Contact.DateOfBirth ?? storedApplicationUser.Profile.Contact.DateOfBirth;
        storedApplicationUser.Profile.Contact.HomePhone = appUser.Profile.Contact.HomePhone ?? storedApplicationUser.Profile.Contact.HomePhone;
        storedApplicationUser.Profile.Contact.MobilePhone = appUser.Profile.Contact.MobilePhone ?? storedApplicationUser.Profile.Contact.MobilePhone;
        storedApplicationUser.Profile.Contact.Website = appUser.Profile.Contact.Website ?? storedApplicationUser.Profile.Contact.Website;

        // Recalculate derived fields
        storedApplicationUser.Profile.Contact.GenerateAddress();
        storedApplicationUser.Profile.Contact.GenerateAge();
        storedApplicationUser.ModifiedAt = DateTime.UtcNow;

        // Save updates through the service
        await _cosmosDbService.UpdateItemAsync(userId, storedApplicationUser);

        return Ok(storedApplicationUser);
    }

    [HttpDelete]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> DeleteAsync()
    {
        string userId = GetUserId().ToString();
        var applicationUserToDelete = await _cosmosDbService.GetItemAsync<AppUser>(userId);

        if (applicationUserToDelete is null)
        {
            return NotFound();
        }

        // Delete the user through the service
        await _cosmosDbService.DeleteItemAsync(userId);
        return NoContent();
    }
}
