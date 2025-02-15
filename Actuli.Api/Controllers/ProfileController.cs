using System.ComponentModel.DataAnnotations;
using Actuli.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Actuli.Api.Models.ProfileTypes;

namespace Actuli.Api.Controllers;

[Authorize]
[Route("api/user/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IAppUserService _appUserService;

    public ProfileController(IAppUserService appUserService)
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
    private bool RequestCanAccessAppUser(Guid userId)
    {
        return IsAppMakingRequest() || (userId == GetUserId());
    }

    [HttpPut("contact")]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> UpdateContact([FromBody, Required] Contact contact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Check for null input
        if (contact is null)
        {
            return BadRequest("Contact is null.");
        }
    
        string userId = GetUserId().ToString();
        var storedAppUser = await _appUserService.GetUserByIdAsync(userId);

        // Handle user not found
        if (storedAppUser is null)
        {
            return NotFound("User not found.");
        }

        // Ensure Contact is not null
        if (storedAppUser.Profile.Contact == null)
        {
            storedAppUser.Profile.Contact = new Contact();
        }

        // Update fields with non-null values from input
        UpdateContactFields(storedAppUser.Profile.Contact, contact);

        // Ensure derived values are updated
        storedAppUser.Profile.Contact.GenerateAddress();
        storedAppUser.Profile.Contact.GenerateAge();

        // Mark user as modified
        storedAppUser.MarkAsModified();

        // Save updates through the service
        await _appUserService.UpdateUserAsync(userId, storedAppUser);

        return Ok(storedAppUser);
    }

    private void UpdateContactFields(Contact target, Contact source)
    {
        target.Email = source.Email ?? target.Email;
        target.FirstName = source.FirstName ?? target.FirstName;
        target.LastName = source.LastName ?? target.LastName;
        target.Address1 = source.Address1 ?? target.Address1;
        target.Address2 = source.Address2 ?? target.Address2;
        target.City = source.City ?? target.City;
        target.State = source.State ?? target.State;
        target.PostalCode = source.PostalCode ?? target.PostalCode;
        target.Country = source.Country ?? target.Country;
        target.DateOfBirth = source.DateOfBirth ?? target.DateOfBirth;
        target.HomePhone = source.HomePhone ?? target.HomePhone;
        target.MobilePhone = source.MobilePhone ?? target.MobilePhone;
        target.Website = source.Website ?? target.Website;
    }
}
