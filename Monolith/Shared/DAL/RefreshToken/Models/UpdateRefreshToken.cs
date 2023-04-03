using Spotify.Shared.tools;

namespace Spotify.Shared.DAL.RefreshToken.Models;

public record UpdateRefreshToken
{
    public Optional<string?> Token { get; set; }
}