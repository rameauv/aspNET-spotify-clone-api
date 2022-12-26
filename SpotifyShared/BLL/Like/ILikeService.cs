namespace Spotify.Shared.BLL.Like;

public interface ILikeService
{
    Task DeleteAsync(string id);
}