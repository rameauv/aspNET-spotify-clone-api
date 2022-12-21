namespace Spotify.Shared.BLL.Release.Models;

public class Release
{
    public string Id { get; set; }
    public string Title { get; set; }
    public DateOnly ReleaseDate { get; set; }
    public string ThumbnailUrl { get; set; }
    public string ArtistThumbnailUrl { get; set; }
    public string AlbumType { get; set; }
    public bool IsLiked { get; set; }
}