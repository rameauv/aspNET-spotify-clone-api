using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Newtonsoft.Json;
using SystemTests.Models;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the Track API.
/// </summary>
[Collection("system tests")]
public class TrackTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrackTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public TrackTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    /// <summary>
    /// Tests that the API returns an track when provided a valid track ID.
    /// </summary>
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

    /// <summary>
    /// Tests that the API returns a bad request error when provided an invalid track ID.
    /// </summary>
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

    /// <summary>
    /// Tests the scenario where the API should set the like status for the track when provided a valid track ID.
    /// </summary>
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
    
    /// <summary>
    /// Tests the scenario where the API should set the like status for the track when provided a valid track ID only for the current user.
    /// </summary>
    [Fact]
    public async Task ShouldSetAndRetrieveTheLikeStatusOfAnTrackOnlyForTheCurrentUser()
    {
        const string id = "2noRn2Aes5aoNVsU6iWThc";

        var client = Factory.CreateClient();
        await InitDb();
        var currentUserTestUserCredentials = new TestUserCredentials("currentUser", "currentUserPassword");
        var secondUserTestUserCredentials = new TestUserCredentials("secondUser", "secondUserPassword");
        await CreateTestUser(currentUserTestUserCredentials);
        await CreateTestUser(secondUserTestUserCredentials);

        await Login(currentUserTestUserCredentials.Username, currentUserTestUserCredentials.Password, client);
        var setLikeResponse = await client.PatchAsJsonAsync($"Track/{id}/Like", "");
        setLikeResponse.EnsureSuccessStatusCode();
        var logoutResponse = await client.PostAsJsonAsync("Accounts/Logout", "");
        logoutResponse.EnsureSuccessStatusCode();
        await Login(secondUserTestUserCredentials.Username, secondUserTestUserCredentials.Password, client);
        var trackResponse = await client.GetAsync($"Track/{id}");
        trackResponse.EnsureSuccessStatusCode();
        var serializedTrackDto = await trackResponse.Content.ReadAsStringAsync();
        var trackDto = JsonConvert.DeserializeObject<TrackDto>(serializedTrackDto);

        Assert.NotNull(trackDto);
        Assert.Null(trackDto.LikeId);
    }
}