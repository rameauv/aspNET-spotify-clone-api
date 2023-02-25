namespace Spotify.Shared.BLL.Album.Models;

public record AlbumTrack(string Id, string Title, string ArtistName)
{
    public string Id { get; set; } = Id;
    public string Title { get; set; } = Title;
    public string ArtistName { get; set; } = ArtistName;
}