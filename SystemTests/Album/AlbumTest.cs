using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Castle.Components.DictionaryAdapter.Xml;
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
    public async Task ShouldGetAnAlbumById()
    {
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var albumRequestResponse = await client.GetAsync($"Album/2noRn2Aes5aoNVsU6iWThc");
        albumRequestResponse.EnsureSuccessStatusCode();
        var serializedAlbumDto = await albumRequestResponse.Content.ReadAsStringAsync();
        var albumDto = JsonConvert.DeserializeObject<AlbumDto>(serializedAlbumDto);
        Assert.NotNull(albumDto?.Id);
    }

    /// <summary>
    /// Tests that the API returns a bad request error when provided an invalid album ID.
    /// </summary>
    [Fact]
    public async Task ShouldGetABadRequestErrorWhenGettingAnAlbumWithAnInvalidId()
    {
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var albumRequestResponse = await client.GetAsync($"Album/notAValidId");
        Assert.Equal(HttpStatusCode.BadRequest, albumRequestResponse.StatusCode);
    }

    /// <summary>
    /// Tests the scenario where the API should return the associated tracks of an album when provided a valid album ID.
    /// </summary>
    [Fact]
    public async Task ShouldGetTheAssociatedTracksOfAnAlbum()
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
        Assert.NotNull(albumTracksDto?.Items);
    }

    /// <summary>
    /// Tests the scenario where the API should return a Bad Request error when attempting to retrieve the associated tracks of an album with an invalid ID.
    /// </summary>
    [Fact]
    public async Task ShouldGetABadRequestErrorWhenGettingAnAlbumSTracksWithAnInvalidId()
    {
        const string id = "aaaa";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var albumTracksRequestResponse = await client.GetAsync($"Album/{id}/Tracks");
        Assert.Equal(HttpStatusCode.BadRequest, albumTracksRequestResponse.StatusCode);
    }

    /// <summary>
    /// Tests the scenario where the API should set the like status for the album when provided a valid album ID.
    /// </summary>
    [Fact]
    public async Task ShouldSetTheLikeStatusForTheAlbum()
    {
        const string id = "2noRn2Aes5aoNVsU6iWThc";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var setLikeRequestResponse = await client.PatchAsJsonAsync($"Album/{id}/Like", "");
        var serializedLikeDto = await setLikeRequestResponse.Content.ReadAsStringAsync();
        var likeDto = JsonConvert.DeserializeObject<LikeDto>(serializedLikeDto);
        setLikeRequestResponse.EnsureSuccessStatusCode();
        var albumRequestResponse = await client.GetAsync($"Album/{id}");
        var serializedAlbumDto = await albumRequestResponse.Content.ReadAsStringAsync();
        var albumDto = JsonConvert.DeserializeObject<AlbumDto>(serializedAlbumDto);

        Assert.NotNull(likeDto?.Id);
        Assert.NotNull(albumDto?.LikeId);
        Assert.Equal(albumDto.LikeId, likeDto.Id);
    }
}