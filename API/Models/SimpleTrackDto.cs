using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Api.Models;

public class SimpleTrackDto
{
    public SimpleTrackDto(string id, string title, string artistName)
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