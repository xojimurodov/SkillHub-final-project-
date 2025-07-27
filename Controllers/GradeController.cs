using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using SkillHub.Models;

[ApiController]
[Route("api/[controller]")]
public class GradeController : ControllerBase
{
    private readonly IGradeService _gradeService;

    public GradeController(IGradeService gradeService)
    {
        _gradeService = gradeService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] GradeCreateDto dto)
    {
        try
        {
            if (!User.IsInRole("Mentor"))
                return BadRequest("You are not authorized to create grades.");
            if (dto == null)
                return BadRequest("Grade data is required.");

            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _gradeService.CreateAsync(teacherId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to create grade.", details = ex.Message });
        }
    }

    [HttpPut("{gradeId}")]
    public async Task<IActionResult> Update(int gradeId, [FromBody] GradeUpdateDto dto)
    {
        try
        {
            if (!User.IsInRole("Mentor"))
                return BadRequest("You are not authorized to create grades.");
            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _gradeService.UpdateAsync(gradeId, teacherId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to update grade.", details = ex.Message });
        }
    }

    [HttpDelete("{gradeId}")]
    public async Task<IActionResult> Delete(int gradeId)
    {
        try
        {
            if (!User.IsInRole("Mentor"))
                return BadRequest("You are not authorized to create grades.");

            var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _gradeService.DeleteAsync(gradeId, teacherId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to delete grade.", details = ex.Message });
        }
    }

    [HttpGet("top-students")]
    public async Task<IActionResult> GetTopStudents()
    {
        try
        {
            if (_gradeService == null)
            {
                return NotFound("Grade service not found.");
            }

            var result = await _gradeService.GetTopLearnersAsync(10);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve top students.", details = ex.Message });
        }
    }


}
