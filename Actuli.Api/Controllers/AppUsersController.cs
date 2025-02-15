using Actuli.Api.Interfaces;
using Actuli.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Actuli.Api.Controllers;

[ApiController]
[Route("api/users")]
public class AppUsersController : ControllerBase
{
    private readonly IAppUserService _appUserService;

    public AppUsersController(IAppUserService appUserService)
    {
        _appUserService = appUserService;
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> CreateItem(string id, [FromBody] AppUser appUser)
    {
        appUser.Id = id;
        await _appUserService.AddUserAsync(appUser);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(string id)
    {
        var appUser = await _appUserService.GetUserByIdAsync(id);
        if (appUser == null) return NotFound();
        return Ok(appUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var items = await _appUserService.GetAllUsersAsync();
        return Ok(items);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(string id, [FromBody] AppUser appUser)
    {
        appUser.Id = id;
        await _appUserService.UpdateUserAsync(id, appUser);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        await _appUserService.DeleteUserAsync(id);
        return NoContent();
    }
}