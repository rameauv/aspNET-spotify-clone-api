using System.Net;
using System.Net.Http.Json;
using Api.Models;
using Newtonsoft.Json;
using SystemTests.Models;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the Artist API.
/// </summary>
[Collection("system tests")]
public class ArtistTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArtistTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public ArtistTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    /// <summary>
    /// Tests that the API returns an artist when provided a valid artist ID.
    /// </summary>
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

    /// <summary>
    /// Tests that the API returns a bad request error when provided an invalid artist ID.
    /// </summary>
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
    
    /// <summary>
    /// Tests the scenario where the API should set the like status for the artist when provided a valid artist ID.
    /// </summary>
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
    
    /// <summary>
    /// Tests the scenario where the API should set the like status for the artist when provided a valid artist ID only for the current user.
    /// </summary>
    [Fact]
    public async Task ShouldSetAndRetrieveTheLikeStatusOfAnArtistOnlyForTheCurrentUser()
    {
        const string id = "4tZwfgrHOc3mvqYlEYSvVi";

        var client = Factory.CreateClient();
        await InitDb();
        var currentUserTestUserCredentials = new TestUserCredentials("currentUser", "currentUserPassword");
        var secondUserTestUserCredentials = new TestUserCredentials("secondUser", "secondUserPassword");
        await CreateTestUser(currentUserTestUserCredentials);
        await CreateTestUser(secondUserTestUserCredentials);

        await Login(currentUserTestUserCredentials.Username, currentUserTestUserCredentials.Password, client);
        var setLikeResponse = await client.PatchAsJsonAsync($"Artist/{id}/Like", "");
        setLikeResponse.EnsureSuccessStatusCode();
        var logoutResponse = await client.PostAsJsonAsync("Accounts/Logout", "");
        logoutResponse.EnsureSuccessStatusCode();
        await Login(secondUserTestUserCredentials.Username, secondUserTestUserCredentials.Password, client);
        var artistResponse = await client.GetAsync($"Artist/{id}");
        artistResponse.EnsureSuccessStatusCode();
        var serializedArtistDto = await artistResponse.Content.ReadAsStringAsync();
        var artistDto = JsonConvert.DeserializeObject<ArtistDto>(serializedArtistDto);

        Assert.NotNull(artistDto);
        Assert.Null(artistDto.LikeId);
    }
}