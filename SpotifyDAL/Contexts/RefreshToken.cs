namespace Repositories.Contexts;

public class RefreshToken
{
    public Guid UserId { get; set; }

    public Guid DeviceId { get; set; }

    public string Token { get; set; }

    public virtual User User { get; set; } = null!;
}