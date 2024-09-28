using CRMAuth.Models;

namespace CRMAuth.Services;

public interface IUserService
{
    Task<int> CreateUserAsync(CreateUserDto user);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<bool> UpdateUserAsync(int id, UpdateUserDto user);
    Task<List<UserDto>> GetAllUsersAsync();
    Task<User?> GetUserByEmail(string email);
    bool VerifyPassword(string password, string hashedPassword);
    string GenerateJwtToken(User user);
}