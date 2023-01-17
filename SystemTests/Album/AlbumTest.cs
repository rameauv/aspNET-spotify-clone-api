using System.Net;
using Api.Models;
using Newtonsoft.Json;

namespace SystemTests.Album;

/// <summary>
/// Test class for testing the Album API.
/// </summary>
public class AlbumTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AlbumTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public AlbumTest(CustomWebApplicationFactory<Program> applicationFactory)
        : base(applicationFactory)
    {
    }

    /// <summary>
    /// Tests that the API returns an album when provided a valid album ID.
    /// </summary>
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
        var albumDto = JsonConvert.DeserializeObject<AlbumDto>(serializedAlbumDto);
        Assert.NotNull(albumDto);
    }

    /// <summary>
    /// Tests that the API returns a bad request error when provided an invalid album ID.
    /// </summary>
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

    [Fact]
    public async void ShouldGetTheAssociatedTracksOfAnAlbum()
    {
        const string id = "2noRn2Aes5aoNVsU6iWThc";
        
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var albumTracksRequestResponse = await client.GetAsync($"Album/{id}/Tracks");
        albumTracksRequestResponse.EnsureSuccessStatusCode();
        var serializedAlbumTracksDto = await albumTracksRequestResponse.Content.ReadAsStringAsync();
        var albumTracksDto = JsonConvert.DeserializeObject<AlbumTracksDto>(serializedAlbumTracksDto);
        Assert.NotNull(albumTracksDto);
    }

    [Fact]
    public async void ShouldGetABadRequestErrorWhenGettingAnAlbumSTracksWithAnInvalidId()
    {
        const string id = "aaaa";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);
        
        var albumTracksRequestResponse = await client.GetAsync($"Album/{id}/Tracks");
        Assert.Equal(HttpStatusCode.BadRequest, albumTracksRequestResponse.StatusCode);
    }
}