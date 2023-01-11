using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class JwtAuthenticationDefaults
{
    [Required]
    public const string AuthenticationScheme = "JWT";
    [Required]
    public const string HeaderName = "Authorization";
    [Required]
    public const string BearerPrefix = "Bearer";
}