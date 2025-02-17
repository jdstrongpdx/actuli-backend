using Actuli.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Actuli.Api.Controllers;

[ApiController]
[Route("api/types")]
public class TypeDataController : ControllerBase
{
    private readonly ITypeDataService _typeDataService;

    public TypeDataController(ITypeDataService typeDataService)
    {
        _typeDataService = typeDataService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTypeData()
    {
        var items = await _typeDataService.GetAllTypesAsync();
        return Ok(items);
    }
    
    [HttpGet("update")]
    public async Task<IActionResult> UpdateTypeData()
    {
        await _typeDataService.UpdateTypesAsync();
        return Ok();
    }
}