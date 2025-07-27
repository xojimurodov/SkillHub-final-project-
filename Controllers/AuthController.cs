using Microsoft.AspNetCore.Mvc;
using SkillHub.DTOs;
using SkillHub.Interfaces;

namespace SkillHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var token = await _authService.RegisterAsync(dto);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Registration failed.", details = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var token = await _authService.LoginAsync(dto);
            return Ok(new { token });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Login failed.", details = ex.Message });
        }
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        if (_authService == null)
        {
            return NotFound("Auth service not found.");
        }
        try
        {
            var users = await _authService.GetAllAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to retrieve users.", details = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        try
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid("You are not authorized to delete users.");
            }
            else if (id <= 0)
            {
                return BadRequest("Invalid user ID.");
            }
            if (_authService == null)
            {
                return NotFound("Auth service not found.");
            }
            var result = await _authService.DeleteAsync(id);
            if (result == "User not found")
            {
                return NotFound(result);
            }
            return Ok("User deleted successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Failed to delete user.", details = ex.Message });
        }
    }
}
