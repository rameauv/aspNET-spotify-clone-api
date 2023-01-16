using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models;

public record NewAccessTokenDto(string AccessToken)
{
    [Required]
    [JsonPropertyName("accessToken")]
    public string AccessToken { get; set; } = AccessToken;
}