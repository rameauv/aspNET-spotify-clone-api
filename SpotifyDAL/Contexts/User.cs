namespace Repositories.Contexts;

public class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Data { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();
}
