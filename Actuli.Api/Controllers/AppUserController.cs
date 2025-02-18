using Actuli.Api.Interfaces;
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
    private readonly IAppUserService _appUserService;

    public AppUserController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
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
    /// 'roles' (for app permissions) claim are not to be honored.
    ///
    /// An access token issued by Azure AD will have at least one of the two claims. Access tokens
    /// issued to a user will have the 'scp' claim. Access tokens issued to an app will have
    /// the roles claim. Access tokens that contain both claims are issued only to users, where the scp
    /// claim designates the delegated permissions, while the roles claim designates the user's role.
    ///
    /// To determine whether an access token was issued to a user (i.e delegated) or an app
    /// more easily, we recommend enabling the optional claim 'idtyp'. For more information, see:
    /// https://docs.microsoft.com/azure/active-directory/develop/access-tokens#user-and-app-tokens
    /// </summary>
    private bool IsAppMakingRequest()
    {
        // Add in the optional 'idtyp' claim to check if the access token is coming from an app or user.
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
    private bool RequestCanAccessAppUser(Guid userId)
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
        AppUser appUser = await _appUserService.GetUserByIdAsync(userId);

        if (appUser is null)
        {
            try
            {
                // TODO: NEED TO CREATE USER ON SIGNUP AND REMOVE THIS SECTION
                appUser = new AppUser(userId);
                await _appUserService.AddUserAsync(appUser);
            }
            catch (CosmosException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Request Diagnostics: {ex.Diagnostics}");
            }
        }

        return Ok(appUser);
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
        var storedAppUser = await _appUserService.GetUserByIdAsync(userId);

        if (storedAppUser is null)
        {
            return NotFound("User not found.");
        }

        storedAppUser = appUser;

        // Save updates through the service
        await _appUserService.UpdateUserAsync(userId, storedAppUser);

        return Ok(storedAppUser);
    }

    [HttpDelete]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> DeleteAsync()
    {
        string userId = GetUserId().ToString();
        var appUserToDelete = await _appUserService.GetUserByIdAsync(userId);

        if (appUserToDelete is null)
        {
            return NotFound();
        }

        // Delete the user through the service
        await _appUserService.DeleteUserAsync(userId);
        return NoContent();
    }
}
