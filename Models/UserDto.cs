using System.ComponentModel.DataAnnotations;

namespace CRMAuth.Models;

public class UserDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
}

public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }
    [Required]
    [MaxLength(450)]
    public string Password { get; set; }
    [MaxLength(450)]
    public string? AvatarUrl { get; set; }
}

public class UpdateUserDto
{
    [MaxLength(100)]
    public string Name { get; set; }
    [MaxLength(450)]
    public string? AvatarUrl { get; set; }
    // [EmailAddress]
    // [MaxLength(100)]
    // public string Email { get; set; }
    // [MaxLength(450)]
    // public string Password { get; set; } // to add next time
}