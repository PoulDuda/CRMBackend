﻿using CRMAuth.Models;

namespace CRMAuth.Services;

public interface IUserService
{
    Task<ApiResponse<int>> CreateUserAsync(CreateUserDto user);
    Task<ApiResponse<UserDto?>> GetUserByIdAsync(int id);
    Task<ApiResponse<bool>> UpdateUserAsync(int id, UpdateUserDto user);
    Task<ApiResponse<List<UserDto>>> GetAllUsersAsync();
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
    Task<User?> GetUserByEmail(string email);
    bool VerifyPassword(string password, string hashedPassword);
    string GenerateJwtToken(User user);
    public Task UpdateJwtTokenAsync(int userId, string token);
}