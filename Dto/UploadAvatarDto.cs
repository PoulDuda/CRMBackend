using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CRMAuth.Models;

public class UploadAvatarDto
{
    [Required]
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [Required]
    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }
}