namespace Spotify.Shared.BLL.MyIdentity.Models;

public record RefreshToken(Guid UserId, Guid DeviceId, string? Token)
{
    public Guid UserId { get; set; } = UserId;

    public Guid DeviceId { get; set; } = DeviceId;

    public string? Token { get; set; } = Token;
}