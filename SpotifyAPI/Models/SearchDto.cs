namespace SpotifyApi.Models;

public class SearchDto
{
    public string Query { get; set; }
    public uint? Page { get; set; }
    public uint? PerPage { get; set; }
}
