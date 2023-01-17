using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
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

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    protected TestBase(CustomWebApplicationFactory<Program> applicationFactory)
    {
        Factory = applicationFactory;
    }

    /// <summary>
    /// Initializes the database by truncating the tables named "Likes", "RefreshTokens", "Users".
    /// </summary>
    protected async Task InitDb()
    {
        if (Factory.Services.GetService(typeof(IConfiguration)) is not IConfiguration config)
        {
            throw new NullReferenceException("could not get the config");
        }
        var connectionString = config.GetConnectionString("DBContext");
        await using var db = new NpgsqlConnection(connectionString);
        db.Open();
        await using var cmd = new NpgsqlCommand("TRUNCATE \"Likes\", \"RefreshTokens\", \"Users\";", db);
        cmd.ExecuteNonQuery();
    }

    /// <summary>
    /// Creates a new test user.
    /// </summary>
    /// <returns>The test user credentials.</returns>
    protected async Task<TestUserCredentials> CreateTestUser()
    {
        var credentials = new TestUserCredentials(
            "testUsername",
            "testPassword"
        );
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
    protected async Task Login(string username, string password, HttpClient client)
    {
        var credentials = new LoginCredentialsDto(username, password);

        var loginResponse = await client.PostAsJsonAsync("/Accounts/Login", credentials);
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
        var accessToken = JsonSerializer.Deserialize<NewAccessTokenDto>(loginResponseString);
        Assert.NotNull(accessToken);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
    }
}