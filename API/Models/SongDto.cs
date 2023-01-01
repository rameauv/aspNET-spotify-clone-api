namespace Api.Models;

public interface SongDto
{
    public Guid Id { get; set; }
    public Guid AlbumId { get; set; }
    public Guid ArtistId { get; set; }
    public string Title { get; set; }
    public string ArtistName { get; set; }
    public string AlbumName { get; set; }
    public string ThumbnailUrl { get; set; }
    public bool Isliked { get; set; }
}