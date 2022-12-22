using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RealSpotifyDAL;
using RealSpotifyDAL.Repositories;
using Repositories.Repositories;
using Spotify.BLL.Services;
using Spotify.Models.BLL.Contracts;
using Spotify.Shared;
using Spotify.Shared.BLL.Search;
using Spotify.Shared.DAL.Contracts;
using Spotify.Shared.DAL.Search;
using Spotify.Shared.MyIdentity.Contracts;
using SpotifyApi.AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddSingleton<MySpotifyClient, MySpotifyClient>();

// BLL Dependencies
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMyIdentityService, MyIdentityService>();
builder.Services.AddScoped<ISearchService, SearchService>();


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