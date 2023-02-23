using System.Net.Http.Json;
using Api;
using Api.Models;
using Newtonsoft.Json;
using SystemTests.Models;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the Like API.
/// </summary>
[Collection("system tests")]
public class LikeTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LikeTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public LikeTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    /// <summary>
    /// Tests the scenario where the API should properly remove a like
    /// </summary>
    [Fact]
    public async Task ShouldRemoveALike()
    {
        var artistId = "4tZwfgrHOc3mvqYlEYSvVi";
        var client = Factory.CreateClient();
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var likeResponse = await client.PatchAsJsonAsync($"Artist/{artistId}/Like", "");
        likeResponse.EnsureSuccessStatusCode();
        var serializedLikeDto = await likeResponse.Content.ReadAsStringAsync();
        var likeDto = JsonConvert.DeserializeObject<LikeDto>(serializedLikeDto);
        Assert.NotNull(likeDto);
        var deleteLikeResponse = await client.DeleteAsync($"Like/{likeDto.Id}/Delete");
        deleteLikeResponse.EnsureSuccessStatusCode();
        var artistResponse = await client.GetAsync($"Artist/{artistId}");
        artistResponse.EnsureSuccessStatusCode();
        var serializedArtistDto = await artistResponse.Content.ReadAsStringAsync();
        var artistDto = JsonConvert.DeserializeObject<ArtistDto>(serializedArtistDto);
        
        Assert.NotNull(artistDto);
        Assert.Null(artistDto.LikeId);
    }
    
    /// <summary>
    /// Tests the scenario where the API should set the like status for the album when provided a valid album ID only for the current user.
    /// </summary>
    [Fact]
    public async Task ShouldSetAndRetrieveTheLikeStatusOfAnAlbumOnlyForTheCurrentUser()
    {
        var artistId = "4tZwfgrHOc3mvqYlEYSvVi";

        var client = Factory.CreateClient();
        await InitDb();
        var currentUserTestUserCredentials = new TestUserCredentials("currentUser", "currentUserPassword");
        var secondUserTestUserCredentials = new TestUserCredentials("secondUser", "secondUserPassword");
        await CreateTestUser(currentUserTestUserCredentials);
        await CreateTestUser(secondUserTestUserCredentials);

        // set the like status for the second user and logout
        await Login(secondUserTestUserCredentials.Username, secondUserTestUserCredentials.Password, client);
        var secondUserLikeResponse = await client.PatchAsJsonAsync($"Artist/{artistId}/Like", "");
        secondUserLikeResponse.EnsureSuccessStatusCode();
        var secondUserLogoutResponse = await client.PostAsJsonAsync("Accounts/Logout", "");
        secondUserLogoutResponse.EnsureSuccessStatusCode();
        
        // set the like status for the current user and logout
        await Login(currentUserTestUserCredentials.Username, currentUserTestUserCredentials.Password, client);
        var currentUserLikeResponse = await client.PatchAsJsonAsync($"Artist/{artistId}/Like", "");
        currentUserLikeResponse.EnsureSuccessStatusCode();
        var serializedLikeDto = await currentUserLikeResponse.Content.ReadAsStringAsync();
        var likeDto = JsonConvert.DeserializeObject<LikeDto>(serializedLikeDto);
        Assert.NotNull(likeDto);
        var deleteLikeResponse = await client.DeleteAsync($"Like/{likeDto.Id}/Delete");
        deleteLikeResponse.EnsureSuccessStatusCode();
        var currentUserLogoutResponse = await client.PostAsJsonAsync("Accounts/Logout", "");
        currentUserLogoutResponse.EnsureSuccessStatusCode();
        
        // get the like status of the album for the second user
        await Login(secondUserTestUserCredentials.Username, secondUserTestUserCredentials.Password, client);
        var artistResponse = await client.GetAsync($"Artist/{artistId}");
        artistResponse.EnsureSuccessStatusCode();
        var serializedArtistDto = await artistResponse.Content.ReadAsStringAsync();
        var artistDto = JsonConvert.DeserializeObject<ArtistDto>(serializedArtistDto);
        
        // ensure that the like status of the album for the second user did not change
        Assert.NotNull(artistDto);
        Assert.NotNull(artistDto.LikeId);
    }
}