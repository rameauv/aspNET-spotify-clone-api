namespace Spotify.Shared.DAL.Like.models;

public class UserLikesRequest
{
    public int? Limit { get; set; } = null;
    public int? Offset { get; set; } = null;
}