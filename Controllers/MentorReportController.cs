using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using System.Security.Claims;

namespace SkillHub.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Mentor")]
public class MentorReportController : ControllerBase
{
    private readonly IMentorReportService _service;

    public MentorReportController(IMentorReportService service)
    {
        _service = service;
    }

    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var report = await _service.GetByIdAsync(id);
        return report == null ? NotFound() : Ok(report);
    }

    [HttpPost]
    public async Task<IActionResult> Create(MentorReportCreateDto dto)
    {
        var mentorId = GetUserId();
        var created = await _service.CreateAsync(mentorId, dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, MentorReportUpdateDto dto)
    {
        var mentorId = GetUserId();
        var updated = await _service.UpdateAsync(id, mentorId, dto);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var mentorId = GetUserId();
        var deleted = await _service.DeleteAsync(id, mentorId);
        return deleted ? NoContent() : NotFound();
    }
}
