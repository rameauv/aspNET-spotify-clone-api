using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SystemTests.Models;

namespace SystemTests;

public class TestBase
{
    protected readonly CustomWebApplicationFactory<Program> Factory;

    protected TestBase(CustomWebApplicationFactory<Program> applicationFactory)
    {
        Factory = applicationFactory;
    }

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