using Microsoft.AspNetCore.Mvc;
using SkillHub.DTOs;
using SkillHub.Interfaces;

namespace SkillHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MentorshipSessionController : ControllerBase
{
    private readonly IMentorshipSessionService _service;

    public MentorshipSessionController(IMentorshipSessionService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MentorshipSessionCreateDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _service.CreateAsync(dto);
            return Ok($"{result.Id} created successfully");
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
            if (_service == null)
            {
                return NotFound("Service not found");
            }
            var sessions = await _service.GetAllAsync();
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, MentorshipSessionCreateDto dto)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }

            var updated = await _service.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }

            return Ok($"{id} updated successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("Invalid ID");
            }
            else if (_service == null)
            {
                return NotFound("Service not found");
            }

            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return Ok($"{id} deleted successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFiltered([FromQuery] List<string>? tags)
    {
        try
        {
            if (tags == null || !tags.Any())
            {
                return BadRequest("Tags cannot be null or empty");
            }
            var sessions = await _service.GetFilteredAsync(tags);
            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
