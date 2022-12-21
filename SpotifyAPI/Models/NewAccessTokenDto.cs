namespace SpotifyApi.Models;

public record NewAccessTokenDto(string AccessToken)
{
    public string AccessToken { get; set; } = AccessToken;
}