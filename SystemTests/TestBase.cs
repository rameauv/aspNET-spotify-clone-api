using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Api;
using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Net.Http.Headers;
using Npgsql;
using SystemTests.Context;
using SystemTests.Models;

namespace SystemTests;

/// <summary>
/// Base class for tests containing common functionality.
/// </summary>
public class TestBase
{
    /// <summary>
    /// The factory used to create clients for making requests to the API.
    /// </summary>
    protected readonly CustomWebApplicationFactory<Program> Factory;

    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    protected TestBase(CustomWebApplicationFactory<Program> applicationFactory)
    {
        Factory = applicationFactory;
        if (Factory.Services.GetService(typeof(IConfiguration)) is not IConfiguration config)
        {
            throw new NullReferenceException("could not get the config");
        }

        var connectionString = config.GetConnectionString("DBContext");
        if (connectionString == null)
        {
            throw new NullReferenceException("could not get the db's connection string");
        }

        _connectionString = connectionString;
    }

    /// <summary>
    /// Initializes the database by truncating the tables named "Likes", "RefreshTokens", "Users".
    /// </summary>
    protected async Task InitDb()
    {
        await using var db = CreateDbConnection();
        db.Open();
        await using var cmd = new NpgsqlCommand("TRUNCATE \"Likes\", \"RefreshTokens\", \"Users\";", db);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Creates a new test user.
    /// </summary>
    /// <returns>The test user credentials.</returns>
    protected Task<TestUserCredentials> CreateTestUser()
    {
        var credentials = new TestUserCredentials(
            "testUsername",
            "testPassword"
        );
        return CreateTestUser(credentials);
    }

    /// <summary>
    /// Creates a new test user.
    /// </summary>
    /// <param name="credentials">Credential of the user to create</param>
    /// <returns>The test user credentials.</returns>
    protected async Task<TestUserCredentials> CreateTestUser(TestUserCredentials credentials)
    {
        var client = Factory.CreateClient();
        var newUser = new CreateUserDto(
            credentials.Username,
            credentials.Password,
            "{}"
        );
        await client.PostAsJsonAsync("/Accounts/Register", newUser);
        return credentials;
    }

    /// <summary>
    /// Logs in with the given credentials and adds the Bearer token to the headers for further request
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <param name="client">The client.</param>
    /// <returns>the refresh token</returns>
    protected async Task<string> Login(string username, string password, HttpClient client)
    {
        var credentials = new LoginCredentialsDto(username, password);

        var loginResponse = await client.PostAsJsonAsync("/Accounts/Login", credentials);
        var serializedNewAccessTokenDto = await loginResponse.Content.ReadAsStringAsync();
        var accessToken = JsonSerializer.Deserialize<NewAccessTokenDto>(serializedNewAccessTokenDto);
        Assert.NotNull(accessToken);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        return _extractRefreshToken(loginResponse);
    }

    protected NpgsqlConnection CreateDbConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    protected TestDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>().UseNpgsql(
            _connectionString);

        return new TestDbContext(options.Options);
    }

    private string _extractRefreshToken(HttpResponseMessage response)
    {
        var cookieNameAndAssignmentSign = "X-Refresh-Token=";
        var setCookies = response.Headers.GetValues(HeaderNames.SetCookie);
        var refreshToken = setCookies.First(setCookie => setCookie.Contains(cookieNameAndAssignmentSign));
        var tokens = refreshToken.Split("; ");
        var expiresToken = tokens.First();
        return expiresToken[cookieNameAndAssignmentSign.Length..];
    }
}