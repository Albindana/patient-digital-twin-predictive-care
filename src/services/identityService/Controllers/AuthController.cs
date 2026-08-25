using Microsoft.AspNetCore.Mvc;
using IdentityService.Entities;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;
using IdentityService.DTOs;

namespace IdentityService.Controllers;

[ApiController]
[Route("api/identity/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IRepository<User> _userRepository;
    private readonly ITokenService _tokenService;

    public AuthController(IRepository<User> userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var existingUsers = await _userRepository.FindAsync(u => u.Email == request.Email);
        if (existingUsers.Any())
        {
            return BadRequest("Email is already registered.");
        }

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            CreatedBy = "System" // We will update this later when we have logged-in users
        };

        await _userRepository.AddAsync(newUser);
        await _userRepository.SaveChangesAsync();

        return Ok(new { Message = "User registered successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var users = await _userRepository.FindAsync(u => u.Email == request.Email);
        var user = users.FirstOrDefault();

        if (user == null)
        {
            return Unauthorized("Invalid email or password.");
        }

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isPasswordValid)
        {
            return Unauthorized("Invalid email or password.");
        }

        var token = _tokenService.GenerateToken(user);

        return Ok(new AuthResponseDto(token, user.Email, user.FirstName));
    }
}