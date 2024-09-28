using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRMAuth.Models;
using CRMAuth.Data;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;

namespace CRMAuth.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<int> CreateUserAsync(CreateUserDto model)
    {
        var user = new User
        {
            Id = 0,
            Name = model.Name,
            Email = model.Email,
            PasswordHash = HashPassword(model.Password),
            AvatarUrl = model.AvatarUrl,
            // to add
            // GoogleCalendarToken
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };
        await _userRepository.AddUserAsync(user);
        return user.Id;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return user == null ? null : new UserDto { Name = user.Name, Email = user.Email, AvatarUrl = user.AvatarUrl };
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.FirstOrDefault(x => x.Email == email);
    }
    
    public async Task<bool> UpdateUserAsync(int id, UpdateUserDto model)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null || model is { Name: "string", AvatarUrl: "string" })
            return false;
        if (model.Name == "string")
        {
            user.AvatarUrl = model.AvatarUrl;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
        else
        {
            user.Name = model.Name;
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(user => new UserDto
            {
                Name = user.Name,
                Email = user.Email,
                AvatarUrl = user.AvatarUrl
            }
        ).ToList();
    }

    public string GenerateJwtToken(User user)
    {
        Env.Load();
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("JWT_KEY"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool VerifyPassword(string enteredPassword, string storedPasswordHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
    }
    
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}