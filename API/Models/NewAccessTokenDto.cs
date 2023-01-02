using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public record NewAccessTokenDto(string AccessToken)
{
    [Required]
    public string AccessToken { get; set; } = AccessToken;
}