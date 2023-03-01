using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Controllers.Album.Models;

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
    [JsonPropertyName("id")]
    public string Id { get; }
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; }
    [Required]
    [JsonPropertyName("releaseDate")]
    public string ReleaseDate { get; }
    [Required]
    [JsonPropertyName("thumbnailUrl")]
    public string? ThumbnailUrl { get; }
    [Required]
    [JsonPropertyName("artistId")]
    public string ArtistId { get; }
    [Required]
    [JsonPropertyName("artistName")]
    public string ArtistName { get; }
    [Required]
    [JsonPropertyName("artistThumbnailUrl")]
    public string? ArtistThumbnailUrl { get; }
    [Required]
    [JsonPropertyName("albumType")]
    public string AlbumType { get; }
    [Required]
    [JsonPropertyName("likeId")]
    public string? LikeId { get; }
}