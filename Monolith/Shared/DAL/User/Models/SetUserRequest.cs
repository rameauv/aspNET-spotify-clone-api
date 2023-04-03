using Spotify.Shared.tools;

namespace Spotify.Shared.DAL.User.Models;

public class SetUserRequest
{
    public Optional<string> Name { get; set; }
}