using CRMAuth.Models;
using CRMAuth.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRMAuth.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
            return NotFound();
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await _userService.GetUserByEmail(loginDto.Email);
        if (user == null || !_userService.VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return Unauthorized();
        }

        var token = _userService.GenerateJwtToken(user);
        return Ok(new { Token = token });
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
    {
        var userId = await _userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUserById), new { id = userId }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto user)
    {
        var result = await _userService.UpdateUserAsync(id, user);
        return result ? NoContent() : NotFound();
    }
}