using System.ComponentModel.DataAnnotations;

namespace Api.Models;

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
        string? likeId
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
        LikeId = likeId;
    }

    [Required]
    public string Id { get; }
    [Required]
    public string Title { get; }
    [Required]
    public string ReleaseDate { get; }
    [Required]
    public string? ThumbnailUrl { get; }
    [Required]
    public string ArtistId { get; }
    [Required]
    public string ArtistName { get; }
    [Required]
    public string? ArtistThumbnailUrl { get; }
    [Required]
    public string AlbumType { get; }
    [Required]
    public string? LikeId { get; }
}