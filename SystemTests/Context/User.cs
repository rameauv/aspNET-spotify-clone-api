namespace SystemTests.Context;

public partial class User
{
    public Guid Id { get; set; }

    public string UserName { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Data { get; set; }
}
