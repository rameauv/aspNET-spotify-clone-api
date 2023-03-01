using System.Net;
using System.Net.Http.Json;
using Api;
using Api.Controllers.Account.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the Account API.
/// </summary>
[Collection("system tests")]
public class AccountTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public AccountTest(CustomWebApplicationFactory<Program> applicationFactory)
        : base(applicationFactory)
    {
    }

    /// <summary>
    /// Tests the login process when providing valid credentials.
    /// </summary>
    [Fact]
    public async Task ShouldLoginWhenProvidingValidCredentials()
    {
        var client = Factory.CreateClient();
        await InitDb();
        var testUserCredentials = await CreateTestUser();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);
    }

    /// <summary>
    /// Tests the login process when providing invalid credentials.
    /// </summary>
    [Fact]
    public async Task ShouldNotLoginWhenProvidingInvalidCredentials()
    {
        var client = Factory.CreateClient();
        await InitDb();

        var credentials = new LoginCredentialsDto("some username", "some password");
        var response = await client.PostAsJsonAsync("Accounts/Login", credentials);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    /// <summary>
    /// Test case that verifies that logging out a user will clear the refresh token.
    /// </summary>
    [Fact]
    public async Task ShouldClearTheRefreshTokenWhenLoggingOut()
    {
        var client = Factory.CreateClient();
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var refreshToken = await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.PostAsJsonAsync("Accounts/Logout", "");

        response.EnsureSuccessStatusCode();
        var setCookies = response.Headers.GetValues(HeaderNames.SetCookie);
        var newRefreshToken = setCookies.First(setCookie => setCookie.Contains("X-Refresh-Token=;"));
        var tokens = newRefreshToken.Split("; ");
        var expiresToken = tokens.First(token => token.Contains("expires"));
        var expiresDateString = expiresToken.Substring(8);
        var expiresDate = DateTime.Parse(expiresDateString);
        Assert.True(expiresDate < DateTime.Now);

        await using var dbContext = CreateDbContext();
        var storedRefreshToken =
            await dbContext.RefreshTokens.FirstOrDefaultAsync(refreshTokenIteration =>
                refreshTokenIteration.Token == refreshToken);
        Assert.Null(storedRefreshToken);
    }
}