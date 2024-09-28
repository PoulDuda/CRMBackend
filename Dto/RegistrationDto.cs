using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace CRMAuth.Models;

public class RegistrationDto
{
    [Required]
    [JsonProperty("id")]
    public int Id { get; set; }
    [Required]
    [JsonProperty("name")]
    public string Name { get; set; }
    [Required]
    [EmailAddress]
    [JsonProperty("email")]
    public string Email { get; set; }
    [JsonProperty("avatar_url", NullValueHandling = NullValueHandling.Ignore)]
    public string? AvatarUrl { get; set; }
    [Required]
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; } 
}