using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Api.Models;
using SystemTests.Fixtures;

namespace SystemTests.Album;

[Collection("Database collection")]
public class UnitTest1 : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly DatabaseFixture _fixture;

    public UnitTest1(DatabaseFixture databaseFixture, CustomWebApplicationFactory<Program> applicationFactory)
        : base(applicationFactory)
    {
        _fixture = databaseFixture;
    }

    [Fact]
    public async void ShouldGetAnAlbumById()
    {
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();
    
        var credentials = new LoginCredentialsDto(testUserCredentials.Username, testUserCredentials.Password);

        var loginResponse = await client.PostAsJsonAsync("/Accounts/Login", credentials);
        var loginResponseString = await loginResponse.Content.ReadAsStringAsync();
        var accessToken = JsonSerializer.Deserialize<NewAccessTokenDto>(loginResponseString);
        Assert.NotNull(accessToken);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
        var response = await client.GetAsync($"Album/2noRn2Aes5aoNVsU6iWThc");
        response.EnsureSuccessStatusCode();
        
    }
}