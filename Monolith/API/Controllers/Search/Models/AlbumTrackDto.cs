using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Controllers.Search.Models;

public class AlbumTrackDto
{
    public AlbumTrackDto(string id, string title, string artistName)
    {
        Id = id;
        Title = title;
        ArtistName = artistName;
    }

    [Required]
    [JsonPropertyName("id")]
    public string Id { get; }
    [Required]
    [JsonPropertyName("title")]
    public string Title { get; }
    [Required]
    [JsonPropertyName("artistName")]
    public string ArtistName { get; }
}