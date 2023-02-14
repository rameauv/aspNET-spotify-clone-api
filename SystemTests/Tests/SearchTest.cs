using Api.Models;
using Newtonsoft.Json;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the Search API.
/// </summary>
[Collection("system tests")]
public class SearchTest: TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
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
        Assert.NotEmpty(searchResultDto.ReleaseResults);
        Assert.NotEmpty(searchResultDto.SongResult);
    }
}
