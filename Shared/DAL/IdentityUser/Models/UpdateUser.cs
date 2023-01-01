using Spotify.Shared.tools;

namespace Spotify.Shared.DAL.IdentityUser.Models;

public record UpdateUser
{
    public Optional<string> Data { get; set; } = new();
}