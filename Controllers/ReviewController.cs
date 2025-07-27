using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillHub.DTOs;
using SkillHub.Interfaces;
using SkillHub.Models;

namespace SkillHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ReviewCreateDto dto)
    {
        if (!User.IsInRole("Learner"))
            return BadRequest("You are not authorized to create reviews.");
        try
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim == null)
            {
                return Unauthorized("Token does not contain user ID.");
            }

            var userId = int.Parse(idClaim.Value);
            var result = await _reviewService.CreateAsync(userId, dto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                error = ex.Message,
                stackTrace = ex.StackTrace
            });
        }
    }


    [HttpGet("session/{sessionId}")]
    public async Task<IActionResult> GetBySession(int sessionId)
    {
        try
        {
            var result = await _reviewService.GetBySessionIdAsync(sessionId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{reviewId}")]
    public async Task<IActionResult> Delete(int reviewId)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            await _reviewService.DeleteAsync(reviewId, userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}
