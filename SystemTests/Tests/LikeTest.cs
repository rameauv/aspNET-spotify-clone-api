using System.Net.Http.Json;
using Api.Models;
using Newtonsoft.Json;

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
}