using Api;
using Api.Controllers.Search.Models;
using Newtonsoft.Json;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the Search API.
/// </summary>
[Collection("system tests")]
public class SearchTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SearchTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public SearchTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    /// <summary>
    /// Verifies that the search API returns any results for a given query.
    /// </summary>
    [Fact]
    public async Task ShouldGetResults()
    {
        await InitDb();
        const string query = "daft punk";

        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Search/Search?q={query}");

        response.EnsureSuccessStatusCode();
        var serializedSearchResultDto = await response.Content.ReadAsStringAsync();
        var searchResultDto = JsonConvert.DeserializeObject<SearchResultDto>(serializedSearchResultDto);
        Assert.NotNull(searchResultDto);
        Assert.NotEmpty(searchResultDto.ArtistResult);
        Assert.NotEmpty(searchResultDto.AlbumResult);
        Assert.NotEmpty(searchResultDto.SongResult);
    }

    /// <summary>
    /// Verifies that the search API returns only artist results for a given query with an artist filter.
    /// </summary>
    [Fact]
    public async Task ShouldGetArtistResults()
    {
        await InitDb();
        const string query = "daft punk";
        const string types = "artist";

        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Search/Search?q={query}&types={types}");

        response.EnsureSuccessStatusCode();
        var serializedSearchResultDto = await response.Content.ReadAsStringAsync();
        var searchResultDto = JsonConvert.DeserializeObject<SearchResultDto>(serializedSearchResultDto);
        Assert.NotNull(searchResultDto);
        Assert.NotEmpty(searchResultDto.ArtistResult);
        Assert.Empty(searchResultDto.AlbumResult);
        Assert.Empty(searchResultDto.SongResult);
    }
    
    /// <summary>
    /// Verifies that the search API returns only artist results for a given query with an artist filter.
    /// </summary>
    [Fact]
    public async Task ShouldGetArtistAndAlbumResults()
    {
        await InitDb();
        const string query = "daft punk";
        const string types = "artist,album";

        var testUserCredentials = await CreateTestUser();
        var client = Factory.CreateClient();

        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync($"Search/Search?q={query}&types={types}");

        response.EnsureSuccessStatusCode();
        var serializedSearchResultDto = await response.Content.ReadAsStringAsync();
        var searchResultDto = JsonConvert.DeserializeObject<SearchResultDto>(serializedSearchResultDto);
        Assert.NotNull(searchResultDto);
        Assert.NotEmpty(searchResultDto.ArtistResult);
        Assert.NotEmpty(searchResultDto.AlbumResult);
        Assert.Empty(searchResultDto.SongResult);
    }
}