using Spotify.Shared.tools;

namespace Spotify.Shared.DAL;

public record UpdateRefreshToken
{
    public Optional<string?> Token { get; set; }
}