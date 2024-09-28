using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CRMAuth.Models;

public class UserDto
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("avatar_url", NullValueHandling = NullValueHandling.Ignore)]
    public string? AvatarUrl { get; set; }
}

public class CreateUserDto
{
    [Required]
    [MaxLength(100)]
    [JsonProperty("name")]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    [MaxLength(100)]
    [JsonProperty("email")]
    public string Email { get; set; }
    [Required]
    [MaxLength(450)]
    [JsonProperty("password")]
    public string Password { get; set; }
    [MaxLength(450)]
    [JsonProperty("avatar_url")]
    public string? AvatarUrl { get; set; }
}

public class UpdateUserDto
{
    [MaxLength(100)]
    [JsonProperty("name")]
    public string Name { get; set; }
    [MaxLength(450)]
    [JsonProperty("avatar_url")]
    public string? AvatarUrl { get; set; }
    // [EmailAddress]
    // [MaxLength(100)]
    // public string Email { get; set; }
    // [MaxLength(450)]
    // public string Password { get; set; } // to add next time
}