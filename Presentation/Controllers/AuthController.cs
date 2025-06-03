using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(SignUpRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.SignUpAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }
    [HttpPost("login")]
    public async Task<IActionResult> LogIn(LogInRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await _authService.LoginAsync(request);
        if (result.Success)
        {
            return Ok(result);
        }
        return Unauthorized(result.Error);
    }
    [HttpPost("signout")]
    public async Task<IActionResult> SignOut()
    {
        var result = await _authService.SignOutAsync();
        if (result.Success)
        {
            return Ok(result);
        }
        return BadRequest(result.Error);
    }
}
