using System.ComponentModel.DataAnnotations;

namespace CRMAuth.Models;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Password { get; set; }
}

public class LoginResponseDto
{
    [Required]
    public string JwtToken { get; set; }
}