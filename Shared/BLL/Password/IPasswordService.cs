namespace Spotify.Shared.BLL.Password;

public interface IPasswordService
{
    public string Hash(string text);
    public bool Verify(string text, string hash);
}