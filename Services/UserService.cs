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
    public async Task<ApiResponse<int>> CreateUserAsync(CreateUserDto model)
    {
        var user = new User
        {
            Id = 0,
            Name = model.Name,
            Email = model.Email,
            PasswordHash = HashPassword(model.Password),
            AvatarUrl = model.AvatarUrl,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = null
        };

        await _userRepository.AddUserAsync(user);

        return new ApiResponse<int>
        {
            StatusCode = 201,
            Message = "User created successfully",
            Data = user.Id
        };
    }

    public async Task<RegistrationDto> GetRegInfo(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        return new RegistrationDto
        {
            id = user.Id,
            name = user.Name,
            avatar_url = user.AvatarUrl,
            created_at = user.CreatedAt,
            email = user.Email
        };
    }
    
    public async Task UpdateJwtTokenAsync(int userId, string token)
    {
        await _userRepository.UpdateJwtTokenAsync(userId, token);
    }

    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        var user = await GetUserByEmail(loginDto.Email);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
        {
            return null;
        }

        var token = GenerateJwtToken(user);
        await _userRepository.UpdateJwtTokenAsync(user.Id, token);

        return new LoginResponseDto { JwtToken = token };
    }

    public async Task<ApiResponse<UserDto?>> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<UserDto?>
            {
                StatusCode = 404,
                Message = "User not found",
                Data = null
            };
        }

        return new ApiResponse<UserDto?>
        {
            StatusCode = 200,
            Message = "Success",
            Data = new UserDto { Name = user.Name, Email = user.Email, AvatarUrl = user.AvatarUrl }
        };
    }
    
    public async Task<User?> GetUserByEmail(string email)
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.FirstOrDefault(x => x.Email == email);
    }
    
    public async Task<ApiResponse<bool>> UpdateUserAsync(int id, UpdateUserDto model)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null)
        {
            return new ApiResponse<bool>
            {
                StatusCode = 404,
                Message = "User not found",
                Data = false
            };
        }

        if (!string.IsNullOrEmpty(model.Name))
            user.Name = model.Name;

        if (!string.IsNullOrEmpty(model.AvatarUrl))
            user.AvatarUrl = model.AvatarUrl;

        user.UpdatedAt = DateTime.UtcNow;
        await _userRepository.UpdateUserAsync(user);

        return new ApiResponse<bool>
        {
            StatusCode = 200,
            Message = "User updated successfully",
            Data = true
        };
    }

    public async Task<ApiResponse<List<UserDto>>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        var userDtos = users.Select(user => new UserDto
        {
            Name = user.Name,
            Email = user.Email,
            AvatarUrl = user.AvatarUrl
        }).ToList();

        return new ApiResponse<List<UserDto>>
        {
            StatusCode = 200,
            Message = "Success",
            Data = userDtos
        };
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