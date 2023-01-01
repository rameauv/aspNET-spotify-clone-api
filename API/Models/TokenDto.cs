namespace Api.Models;

public record TokenDto(string AccessToken, string RefreshToken)
{
    public string AccessToken { get; set; } = AccessToken;
}