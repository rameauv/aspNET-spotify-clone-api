using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RealSpotifyDAL;
using RealSpotifyDAL.Repositories;
using Repositories.Repositories;
using Spotify.BLL.Services;
using Spotify.Shared;
using Spotify.Shared.BLL.Album;
using Spotify.Shared.BLL.Artist;
using Spotify.Shared.BLL.MyIdentity;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.BLL.Track;
using Spotify.Shared.BLL.User;
using Spotify.Shared.DAL.Album;
using Spotify.Shared.DAL.Artist;
using Spotify.Shared.DAL.Contracts;
using Spotify.Shared.DAL.IdentityUser;
using Spotify.Shared.DAL.Search;
using Spotify.Shared.DAL.Track;
using Spotify.Shared.DAL.User;
using SpotifyApi.AutoMapper;
using SpotifyApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition(JwtAuthenticationDefaults.AuthenticationScheme,     
        new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme.",
            Name = JwtAuthenticationDefaults.HeaderName, // Authorization
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtAuthenticationDefaults.AuthenticationScheme
                }
            },
            new List<string>()
        }
    });
});

// Logger
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// CORS
const string myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000");
            policy.AllowCredentials();
            policy.AllowAnyMethod();
            policy.AllowAnyHeader();
        });
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(ApiProfile), typeof(BllProfile));

// Project config
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// DAL Dependencies
builder.Services.AddScoped<IIdentityUserRepository, IdentityUserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddSingleton<MySpotifyClient, MySpotifyClient>();
builder.Services.AddScoped<ITrackRepository, TrackRepository>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// BLL Dependencies
builder.Services.AddScoped<IMyIdentityService, MyIdentityService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ITrackService, TrackService>();
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IUserService, UserService>();
    
var jwtSettingsSection = builder.Configuration.GetSection("Jwt");
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
builder.Services.AddAuthentication(options =>
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
builder.Services.AddAuthorization();


var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // CORS
    app.UseCors(myAllowSpecificOrigins);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();