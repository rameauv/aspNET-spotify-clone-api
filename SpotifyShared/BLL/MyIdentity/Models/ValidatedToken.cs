using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Spotify.Shared.BLL.MyIdentity.Models;

public class ValidatedToken
{
    public ValidatedToken(ClaimsPrincipal principal, SecurityToken token)
    {
        Principal = principal;
        Token = token;
    }

    public ClaimsPrincipal Principal { get; set; }
    public SecurityToken Token { get; set; }
}