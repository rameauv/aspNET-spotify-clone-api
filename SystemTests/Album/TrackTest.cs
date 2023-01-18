using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Newtonsoft.Json;

namespace SystemTests.Album;

/// <summary>
/// Test class for testing the Track API.
/// </summary>
[Collection("system tests")]
public class TrackTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    public TrackTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    [Fact]
    public async Task ShouldGetAnTrackById()
    {
        const string id = "5W3cjX2J3tjhG8zb6u0qHn";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Track/{id}");
        response.EnsureSuccessStatusCode();
        var serializedTrackDto = await response.Content.ReadAsStringAsync();
        var trackDto = JsonConvert.DeserializeObject<TrackDto>(serializedTrackDto);
        Assert.NotNull(trackDto?.Id);
    }

    [Fact]
    public async Task ShouldGetABadRequestErrorWhenGettingAnTrackByProvidingAnInvalidId()
    {
        const string id = "aaaa";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Track/{id}");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ShouldSetTheLikeStatusForTheTrack()
    {
        const string id = "5W3cjX2J3tjhG8zb6u0qHn";

        await InitDb();
        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var setLikeResponse = await client.PatchAsJsonAsync($"Track/{id}/Like", "");
        setLikeResponse.EnsureSuccessStatusCode();
        var serializedLikeDto = await setLikeResponse.Content.ReadAsStringAsync();
        var likeDto = JsonConvert.DeserializeObject<LikeDto>(serializedLikeDto);
        var trackResponse = await client.GetAsync($"Track/{id}");
        var serializedTrackDto = await trackResponse.Content.ReadAsStringAsync();
        var trackDto = JsonConvert.DeserializeObject<TrackDto>(serializedTrackDto);

        Assert.NotNull(likeDto?.Id);
        Assert.NotNull(trackDto?.Id);
        Assert.Equal(likeDto.Id, trackDto.LikeId);
    }
}