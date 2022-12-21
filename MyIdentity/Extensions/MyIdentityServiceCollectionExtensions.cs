using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MyIdentity.Contexts;
using MyIdentity.Services;
using Spotify.Shared;
using Spotify.Shared.MyIdentity.Contracts;

namespace MyIdentity.Extensions;

public static class MyIdentityServiceCollectionExtensions
{
    public static void AddMyIdentity(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<SpotifyContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DBContext")));

        services.AddIdentity<User, IdentityRole<Guid>>()
            .AddEntityFrameworkStores<SpotifyContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        });


        var jwtSettingsSection = configuration.GetSection("Jwt");
        var jwtIssuer = jwtSettingsSection.GetSection("Issuer").Value;
        var jwtAudience = jwtSettingsSection.GetSection("Audience").Value;
        var jwtKey = jwtSettingsSection.GetSection("Key").Value;
        var jwtExpiryInMinutes = jwtSettingsSection.GetSection("ExpiryInMinutes").Value;
        if (jwtIssuer == null
            || jwtAudience == null
            || jwtKey == null
            || jwtExpiryInMinutes == null)
        {
            throw new Exception("the jwt config is missing");
        }

        var jwtSettings = new JwtConfig(
            jwtIssuer,
            jwtAudience,
            jwtKey,
            jwtExpiryInMinutes
        );
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
            };
        });
        services.AddAuthorization();
        services.AddScoped<IMyIdentityService, MyIdentityService>();
    }
}