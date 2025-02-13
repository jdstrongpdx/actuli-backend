using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Actuli.Api.Models;
using Actuli.Api.DbContext;

namespace Actuli.Api.Controllers;

[Authorize]
[Route("api/user")]
[ApiController]
public class ApplicationUserController : ControllerBase
{
    private readonly ApplicationUserDbContext _applicationUserDbContext;

    public ApplicationUserController(ApplicationUserDbContext applicationUserContext)
    {
        _applicationUserDbContext = applicationUserContext;
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
        var applicationUsers = await _applicationUserDbContext.ApplicationUsers!
            .Where(td => RequestCanAccessApplicationUser(td.Owner))
            .ToListAsync();

        return Ok(applicationUsers);
    }

    [HttpGet("{id}")]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Read",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Read"
    )]
    public async Task<IActionResult> GetAsync(int id)
    {
        var applicationUser = await _applicationUserDbContext.ApplicationUsers!
            .FirstOrDefaultAsync(td => RequestCanAccessApplicationUser(td.Owner) && td.Id == id);

        if (applicationUser is null)
        {
            return NotFound();
        }

        return Ok(applicationUser);
    }

    [HttpPut("{id}")]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> PutAsync(int id, [FromBody] ApplicationUser applicationUser)
    {
        var storedApplicationUser = await _applicationUserDbContext.ApplicationUsers!
            .FirstOrDefaultAsync(td => RequestCanAccessApplicationUser(td.Owner) && td.Id == id);

        if (storedApplicationUser is null)
        {
            return NotFound();
        }

        storedApplicationUser.Description = applicationUser.Description;
        _applicationUserDbContext.ApplicationUsers!.Update(storedApplicationUser);

        await _applicationUserDbContext.SaveChangesAsync();

        return Ok(storedApplicationUser);
    }

    [HttpPost]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> PostAsync([FromBody] ApplicationUser applicationUser)
    {
        // Only let applications with global to-do access set the user ID or to-do's
        var ownerIdOfApplicationUser = IsAppMakingRequest() ? applicationUser.Owner : GetUserId();

        var newApplicationUser = new ApplicationUser()
        {
            Owner = ownerIdOfApplicationUser,
            Description = applicationUser.Description
        };

        await _applicationUserDbContext.ApplicationUsers!.AddAsync(newApplicationUser);
        await _applicationUserDbContext.SaveChangesAsync();

        return Created($"/user/{newApplicationUser!.Id}", newApplicationUser);
    }

    [HttpDelete("{id}")]
    [RequiredScopeOrAppPermission(
        RequiredScopesConfigurationKey = "AzureAD:Scopes:Write",
        RequiredAppPermissionsConfigurationKey = "AzureAD:AppPermissions:Write"
    )]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var applicationUserToDelete = await _applicationUserDbContext.ApplicationUsers!
            .FirstOrDefaultAsync(td => RequestCanAccessApplicationUser(td.Owner) && td.Id == id);

        if (applicationUserToDelete is null)
        {
            return NotFound();
        }

        _applicationUserDbContext.ApplicationUsers!.Remove(applicationUserToDelete);

        await _applicationUserDbContext.SaveChangesAsync();

        return Ok();
    }
}