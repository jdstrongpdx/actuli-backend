using Actuli.Api.Interfaces;
using Actuli.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Actuli.Api.Controllers;

[ApiController]
[Route("api/types")]
public class TypeController : ControllerBase
{
    private readonly ITypeService _typeService;

    public TypeController(ITypeService typeService)
    {
        _typeService = typeService;
    }

    // Create an item
    [HttpPost]
    public async Task<IActionResult> CreateItem([FromBody] TypeGroup typeGroup)
    {
        try
        {
            typeGroup.Id = Guid.NewGuid().ToString();
            await _typeService.AddTypeAsync(typeGroup);
            return Ok();
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { Message = "Invalid input: " + ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Message = "An error occurred while creating the item.", Details = ex.Message });
        }
    }

    // Get an item by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetItem(string id)
    {
        try
        {
            var typeGroup = await _typeService.GetTypeByIdAsync(id);
            if (typeGroup == null) return NotFound(new { Message = "Item not found for the given ID." });

            return Ok(typeGroup);
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Message = "An error occurred while retrieving the item.", Details = ex.Message });
        }
    }

// Get all items
    [HttpGet]
    public async Task<IActionResult> GetAllItems()
    {
        try
        {
            var items = await _typeService.GetAllTypesAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Message = "An error occurred while retrieving the items.", Details = ex.Message });
        }
    }

    // Update an item
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateItem(string id, [FromBody] TypeGroup typeGroup)
    {
        try
        {
            await _typeService.UpdateTypeAsync(id, typeGroup);
            return Ok();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = "Item not found for the given ID.", Details = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = "Invalid input: " + ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Message = "An error occurred while updating the item.", Details = ex.Message });
        }
    }

    // Delete an item
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteItem(string id)
    {
        try
        {
            await _typeService.DeleteTypeAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = "Item not found for the given ID.", Details = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500,
                new { Message = "An error occurred while deleting the item.", Details = ex.Message });
        }
    }
}