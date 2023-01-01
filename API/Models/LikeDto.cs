namespace Api.Models;

public class LikeDto
{
    public LikeDto(string id)
    {
        Id = id;
    }

    public string Id { get; set; }
}