using Spotify.Shared.tools;

namespace Spotify.Shared.DAL;

public record UpdateUser
{
    public Optional<string> Data { get; set; } = new();
}