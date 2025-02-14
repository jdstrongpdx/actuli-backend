using Newtonsoft.Json;
using Actuli.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Actuli.Api.Services;

namespace Actuli.Api.DbContext;



using Microsoft.Identity.Web;


[ApiController]
[Route("api/users")]
public class AppUsersController : ControllerBase
{
    private readonly CosmosDbService _cosmosDbService;

    public AppUsersController(CosmosDbService cosmosDbService)
    {
        _cosmosDbService = cosmosDbService;
    }
    
    private Guid GetUserId()
    {
        Guid userId;

        if (!Guid.TryParse(HttpContext.User.GetObjectId(), out userId))
        {
            throw new Exception("User ID is not valid.");
        }

        return userId;
    }

    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] AppUser appUser)
    {
        appUser.Id = "12345";
        await _cosmosDbService.AddItemAsync(appUser);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(string id)
    {
        var appUser = await _cosmosDbService.GetItemAsync<AppUser>(id);
        if (appUser == null) return NotFound();
        return Ok(appUser);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        var items = await _cosmosDbService.GetAllItemsAsync<AppUser>();
        return Ok(items);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(string id, [FromBody] AppUser appUser)
    {
        appUser.Id = id;
        await _cosmosDbService.UpdateItemAsync(id, appUser);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        await _cosmosDbService.DeleteItemAsync(id);
        return NoContent();
    }
}