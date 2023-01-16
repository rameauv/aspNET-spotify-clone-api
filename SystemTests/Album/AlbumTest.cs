using System.Net;
using System.Text.Json;
using Api.Models;

namespace SystemTests.Album;

[Collection("Database collection")]
public class AlbumTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{

    public AlbumTest(CustomWebApplicationFactory<Program> applicationFactory)
        : base(applicationFactory)
    {
    }

    [Fact]
    public async void ShouldGetAnAlbumById()
    {
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var albumRequestResponse = await client.GetAsync($"Album/2noRn2Aes5aoNVsU6iWThc");
        albumRequestResponse.EnsureSuccessStatusCode();
        var serializedAlbumDto = await albumRequestResponse.Content.ReadAsStringAsync();
        var albumDto = JsonSerializer.Deserialize<AlbumDto>(serializedAlbumDto);
        Assert.NotNull(albumDto);
    }

    [Fact]
    public async void ShouldGetABadRequestErrorWhenGettingAnAlbumWithAnInvalidId()
    {
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();
        
        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var albumRequestResponse = await client.GetAsync($"Album/notAValidId");
        Assert.Equal(HttpStatusCode.BadRequest, albumRequestResponse.StatusCode);
    }
}