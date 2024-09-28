using System.ComponentModel.DataAnnotations;

namespace CRMAuth.Models;

public class UploadAvatarDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string AvatarUrl { get; set; }
}