using System.Text.Json.Serialization;

namespace CRMAuth.Models;

public class ApiResponse<T>
{
    [JsonPropertyName("status_code")]
    public int StatusCode { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; }
    [JsonPropertyName("data")]
    public T? Data { get; set; }
}