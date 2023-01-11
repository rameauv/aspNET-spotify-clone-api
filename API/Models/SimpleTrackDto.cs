using System.ComponentModel.DataAnnotations;

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
    public string Id { get; }
    [Required]
    public string Title { get; }
    [Required]
    public string ArtistName { get; }
}