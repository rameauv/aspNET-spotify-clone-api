namespace MyIdentity.Contexts;

public partial class UserLogin
{
    public Guid UserId { get; set; }

    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }
}
