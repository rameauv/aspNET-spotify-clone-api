namespace SpotifyApi.Models;

public class AlbumDto
{
    public AlbumDto(
        string id,
        string title,
        string releaseDate,
        string? thumbnailUrl,
        string artistId,
        string artistName,
        string? artistThumbnailUrl,
        string albumType,
        bool isLiked
        )
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        ThumbnailUrl = thumbnailUrl;
        ArtistId = artistId;
        ArtistName = artistName;
        ArtistThumbnailUrl = artistThumbnailUrl;
        AlbumType = albumType;
        IsLiked = isLiked;
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string ReleaseDate { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string ArtistId { get; set; }
    public string ArtistName { get; set; }
    public string? ArtistThumbnailUrl { get; set; }
    public string AlbumType { get; set; }
    public bool IsLiked { get; set; }
}