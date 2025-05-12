using Microsoft.AspNetCore.Mvc;
using WandShop.Application.Service;
using WandShop.Domain.Models;

namespace WandShopService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FlexibilityController : ControllerBase
{
    private readonly IFlexibilityService _flexibilityService;
    public FlexibilityController(IFlexibilityService flexibilityService)
    {
        _flexibilityService = flexibilityService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Flexibility>>> GetAllFlexibilities()
    {
        var flexibilities = await _flexibilityService.GetAllFlexibilitiesAsync();
        return Ok(flexibilities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Flexibility>> GetFlexibility(int id)
    {
        var flexibility = await _flexibilityService.GetFlexibilityAsync(id);
        if (flexibility == null)
        {
            return NotFound();  
        }
        return Ok(flexibility); 
    }

    [HttpPost]
    public async Task<ActionResult<Flexibility>> AddFlexibility(Flexibility flexibility)
    {
        var createdFlexibility = await _flexibilityService.AddFlexibilityAsync(flexibility);
        return CreatedAtAction(nameof(GetFlexibility), new { id = createdFlexibility.Id }, createdFlexibility);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Flexibility>> UpdateFlexibility(int id, Flexibility flexibility)
    {
        if (id != flexibility.Id)
        {
            return BadRequest();  
        }

        var updatedFlexibility = await _flexibilityService.UpdateFlexibilityAsync(flexibility);
        return Ok(updatedFlexibility);  
    }
}
