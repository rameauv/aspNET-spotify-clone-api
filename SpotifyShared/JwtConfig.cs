namespace Spotify.Shared;

public record JwtConfig(string Issuer, string Audience, string Key, string ExpiryInMinutes)
{
    public string Issuer { get; set; } = Issuer;
    public string Audience { get; set; } = Audience;
    public string Key { get; set; } = Key;
    public string ExpiryInMinutes { get; set; } = ExpiryInMinutes;
}