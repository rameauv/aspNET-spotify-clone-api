namespace Spotify.Shared.DAL.Album.Models;

public record Album(
    string Id,
    string Title,
    string ReleaseDate,
    string? ThumbnailUrl,
    string ArtistId,
    string ArtistName,
    string? ArtistThumbnailUrl,
    string AlbumType
)
{
    public string Id { get; set; } = Id;
    public string Title { get; set; } = Title;
    public string ReleaseDate { get; set; } = ReleaseDate;
    public string? ThumbnailUrl { get; set; } = ThumbnailUrl;
    public string ArtistId { get; set; } = ArtistId;
    public string ArtistName { get; set; } = ArtistName;
    public string? ArtistThumbnailUrl { get; set; } = ArtistThumbnailUrl;
    public string AlbumType { get; set; } = AlbumType;
}