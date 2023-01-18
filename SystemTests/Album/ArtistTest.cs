using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Newtonsoft.Json;

namespace SystemTests.Album;

/// <summary>
/// Test class for testing the Artist API.
/// </summary>
[Collection("system tests")]
public class ArtistTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    public ArtistTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    [Fact]
    public async Task ShouldGetAnArtistById()
    {
        const string id = "4tZwfgrHOc3mvqYlEYSvVi";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Artist/{id}");
        response.EnsureSuccessStatusCode();
        var serializedArtistDto = await response.Content.ReadAsStringAsync();
        var artistDto = JsonConvert.DeserializeObject<ArtistDto>(serializedArtistDto);
        Assert.NotNull(artistDto?.Id);
    }

    [Fact]
    public async Task ShouldGetABadRequestErrorWhenGettingAnArtistByProvidingAnInvalidId()
    {
        const string id = "aaaa";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Artist/{id}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ShouldSetTheLikeStatusForTheArtist()
    {
        const string id = "4tZwfgrHOc3mvqYlEYSvVi";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var setLikeResponse = await client.PatchAsJsonAsync($"Artist/{id}/Like", "");
        setLikeResponse.EnsureSuccessStatusCode();
        var serializedLikeDto = await setLikeResponse.Content.ReadAsStringAsync();
        var likeDto = JsonConvert.DeserializeObject<LikeDto>(serializedLikeDto);
        var artistResponse = await client.GetAsync($"Artist/{id}");
        var serializedArtistDto = await artistResponse.Content.ReadAsStringAsync();
        var artistDto = JsonConvert.DeserializeObject<ArtistDto>(serializedArtistDto);

        Assert.NotNull(likeDto?.Id);
        Assert.NotNull(artistDto?.Id);
        Assert.Equal(likeDto.Id, artistDto.LikeId);
    }
}