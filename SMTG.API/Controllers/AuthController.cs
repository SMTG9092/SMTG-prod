using Microsoft.AspNetCore.Mvc;
using SMTG.API.DTOs;
using SMTG.API.Services;

namespace SMTG.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new
        {
            success = true,
            message = "SMTG API is working"
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.Login(
            request.Email,
            request.Password);

        if (!result.Success)
            return Unauthorized(result);

        return Ok(result);
    }
}