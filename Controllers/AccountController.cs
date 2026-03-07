using Microsoft.AspNetCore.Mvc;
using TutorOS.Api.DTOs;

namespace TutorOS.Api.Controllers;

/// <summary>
/// Handles user account operations such as registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    /// <summary>
    /// Registers a new user and their institute.
    /// </summary>
    /// <param name="registerDto">The registration details.</param>
    /// <returns>A successful response if registration is complete.</returns>
    /// <response code="200">User registered successfully.</response>
    /// <response code="400">Invalid registration details or user already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        // For documentation purposes, we return a mock success response.
        // In a real application, this would call an authentication service.
        return Ok(new AuthResponseDto
        {
            Token = "mock-jwt-token",
            ExpiresAt = DateTime.UtcNow.AddHours(1),
            Email = registerDto.Email
        });
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token.
    /// </summary>
    /// <param name="loginDto">The login credentials.</param>
    /// <returns>An authentication response containing a JWT token.</returns>
    /// <response code="200">Login successful.</response>
    /// <response code="401">Invalid email or password.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        // For documentation purposes, we return a mock success response.
        // In a real application, this would validate credentials against a database or external provider.
        if (loginDto.Email == "admin@tutoros.lk" && loginDto.Password == "password")
        {
            return Ok(new AuthResponseDto
            {
                Token = "mock-jwt-token",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                Email = loginDto.Email
            });
        }

        return Unauthorized("Invalid credentials.");
    }
}
