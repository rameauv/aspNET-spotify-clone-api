namespace Api.Models;

public record NewAccessTokenDto(string AccessToken)
{
    public string AccessToken { get; set; } = AccessToken;
}