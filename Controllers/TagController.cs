using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.DTOs;
using SkillHub.Interfaces;

namespace SkillHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(TagCreateDto dto)
    {
        if (!User.IsInRole("Admin"))
            return BadRequest("You are not authorized to create tags.");
        try
        {
            var tag = await _tagService.CreateAsync(dto);
            return Ok(tag);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            if (_tagService == null)
            {
                return NotFound("Service not found");
            }
            var tags = await _tagService.GetAllAsync();
            return Ok(tags);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _tagService.DeleteAsync(id);

        if (!success)
            return NotFound($"Tag with ID {id} not found");

        return Ok($"Tag {id} deleted successfully");
    }

}
