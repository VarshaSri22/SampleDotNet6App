using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleDotNet6App.Domain.Models;
using SampleDotNet6App.Services.Abstractions;
using System.Security.Claims;

namespace SampleDotNet6App.Api.Controllers;

public record LoginRequest(string Username, string Password);
public record RegisterRequest(string Username, string Password, UserRole Role);

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _userService.LoginAsync(request.Username, request.Password);
        return result.Success ? Ok(new { token = result.Token }) : Unauthorized(new { error = result.Error });
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await _userService.RegisterAsync(request.Username, request.Password, request.Role);
        return user is null
            ? Conflict(new { error = "Username already exists." })
            : CreatedAtAction(nameof(GetById), new { id = user.Id }, new { user.Id, user.Username, user.Role });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var user = await _userService.GetByIdAsync(userId);
        return user is null ? NotFound() : Ok(new { user.Id, user.Username, user.Role });
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll() =>
        Ok((await _userService.GetAllAsync()).Select(u => new { u.Id, u.Username, u.Role }));

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(new { user.Id, user.Username, user.Role });
    }
}
