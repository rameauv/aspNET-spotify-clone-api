using Microsoft.AspNetCore.Identity;

namespace MyIdentity.Contexts;

public class User : IdentityUser<Guid>
{
    public string? RefreshToken { get; set; }

    public User(string userName)
    {
        this.UserName = userName;
    }
}