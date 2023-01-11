using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public record TokenDto(string AccessToken, string RefreshToken)
{
    [Required]
    public string AccessToken { get; set; } = AccessToken;
}