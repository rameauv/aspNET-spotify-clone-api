using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public interface SongDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public Guid AlbumId { get; set; }
    [Required]
    public Guid ArtistId { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public string ArtistName { get; set; }
    [Required]
    public string AlbumName { get; set; }
    [Required]
    public string ThumbnailUrl { get; set; }
    [Required]
    public bool Isliked { get; set; }
}