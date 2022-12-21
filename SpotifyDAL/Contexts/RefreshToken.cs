namespace Repositories.Contexts;

internal partial class RefreshToken
{
    public RefreshToken(Guid userId, Guid deviceId, string token)
    {
        UserId = userId;
        DeviceId = deviceId;
        Token = token;
    }

    public Guid UserId { get; set; }

    public Guid DeviceId { get; set; }

    public string Token { get; set; }

    public virtual User User { get; set; } = null!;
}