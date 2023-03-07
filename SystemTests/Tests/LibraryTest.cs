using System.Net;
using System.Net.Http.Json;
using Api;
using Api.Controllers.Library.Models;
using Newtonsoft.Json;
using SystemTests.Models;

namespace SystemTests.Tests;

[Collection("system tests")]
public class LibraryTest : TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    public LibraryTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    [Fact]
    public async Task AllRoutesShouldBeProtected()
    {
        var client = Factory.CreateClient();

        var libraryRes = await client.GetAsync("Library/CurrentUserLibrary");
        var findLibraryItemsRes = await client.GetAsync("Library/FindLibraryItems");
        var findLikedTracks = await client.GetAsync("Library/FindLikedTracks");

        Assert.Equal(HttpStatusCode.Unauthorized, libraryRes.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, findLibraryItemsRes.StatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, findLikedTracks.StatusCode);
    }

    [Fact]
    public async Task ShouldGetTheCurrentUserLibrary()
    {
        var albumId = "2noRn2Aes5aoNVsU6iWThc";
        var artistId = "4tZwfgrHOc3mvqYlEYSvVi";

        await InitDb();
        var client = Factory.CreateClient();
        var credentials = await CreateTestUser();
        await Login(credentials.Username, credentials.Password, client);

        await client.PatchAsJsonAsync($"Album/{albumId}/Like", "");
        await client.PatchAsJsonAsync($"Artist/{artistId}/Like", "");
        var res = await client.GetAsync("Library/CurrentUserLibrary");

        res.EnsureSuccessStatusCode();
        var libraryDto = JsonConvert.DeserializeObject<LibraryDto>(await res.Content.ReadAsStringAsync());

        Assert.NotNull(libraryDto);
        Assert.Single(libraryDto.Items.Albums.ToArray());
        Assert.Single(libraryDto.Items.Artists.ToArray());
    }

    [Fact]
    public async Task ShouldNotGetTheLibraryOfAnOtherUser()
    {
        var albumId = "2noRn2Aes5aoNVsU6iWThc";

        await InitDb();
        var client = Factory.CreateClient();
        var currentUserCredentials = await CreateTestUser();
        var secondUserCredentials =
            await CreateTestUser(new TestUserCredentials("secondUserUsername", "secondUserPassword"));

        await Login(secondUserCredentials.Username, secondUserCredentials.Password, client);
        await client.PatchAsJsonAsync($"Album/{albumId}/Like", "");
        await client.PostAsJsonAsync("Accounts/Logout", "");

        await Login(currentUserCredentials.Username, currentUserCredentials.Password, client);
        var res = await client.GetAsync("Library/CurrentUserLibrary");
        res.EnsureSuccessStatusCode();
        var libraryDto = JsonConvert.DeserializeObject<LibraryDto>(await res.Content.ReadAsStringAsync());

        Assert.NotNull(libraryDto);
        Assert.Empty(libraryDto.Items.Albums);
    }

    [Fact]
    public async Task ShouldFindTheLibraryItems()
    {
        var albumId = "2noRn2Aes5aoNVsU6iWThc";
        var artistId = "4tZwfgrHOc3mvqYlEYSvVi";

        var client = Factory.CreateClient();
        var credentials = await CreateTestUser();

        await Login(credentials.Username, credentials.Password, client);
        await client.PatchAsJsonAsync($"Album/{albumId}/Like", "");
        await client.PatchAsJsonAsync($"Artist/{artistId}/Like", "");

        var res = await client.GetAsync("Library/FindLibraryItems?limit=2&offset=0");
        res.EnsureSuccessStatusCode();
        var libraryItemsDto = JsonConvert.DeserializeObject<LibraryItemsDto>(await res.Content.ReadAsStringAsync());

        Assert.NotNull(libraryItemsDto);
        Assert.Single(libraryItemsDto.Albums.ToArray());
        Assert.Single(libraryItemsDto.Artists.ToArray());
    }

    [Fact]
    public async Task ShouldNotFindTheLibraryItemsOfOtherUsers()
    {
        var albumId = "2noRn2Aes5aoNVsU6iWThc";

        await InitDb();
        var client = Factory.CreateClient();
        var currentUserCredentials = await CreateTestUser();
        var secondUserCredentials =
            await CreateTestUser(new TestUserCredentials("secondUserUsername", "secondUserPassword"));

        await Login(secondUserCredentials.Username, secondUserCredentials.Password, client);
        await client.PatchAsJsonAsync($"Album/{albumId}/Like", "");
        await client.PostAsJsonAsync("Accounts/Logout", "");

        await Login(currentUserCredentials.Username, currentUserCredentials.Password, client);
        var res = await client.GetAsync("Library/FindLibraryItems?limit=1&offset=0");
        res.EnsureSuccessStatusCode();
        var libraryItemsDto = JsonConvert.DeserializeObject<LibraryItemsDto>(await res.Content.ReadAsStringAsync());

        Assert.NotNull(libraryItemsDto);
        Assert.Empty(libraryItemsDto.Albums);
    }

    [Fact]
    public async Task ShouldFindTheLikedTracks()
    {
        var trackId = "5W3cjX2J3tjhG8zb6u0qHn";

        await InitDb();
        var client = Factory.CreateClient();
        var credentials = await CreateTestUser();

        await Login(credentials.Username, credentials.Password, client);
        await client.PatchAsJsonAsync($"Track/{trackId}/Like", "");
        var res = await client.GetAsync("Library/FindLikedTracks?limit=1&offset=0");
        res.EnsureSuccessStatusCode();
        var findLikedTrackResultDto =
            JsonConvert.DeserializeObject<FindLikedTracksResultDto>(await res.Content.ReadAsStringAsync());

        Assert.NotNull(findLikedTrackResultDto);
        Assert.Single(findLikedTrackResultDto.Items.ToArray());
    }

    [Fact]
    public async Task ShouldNotFindTheLikedTracksOfOtherUsers()
    {
        var trackId = "5W3cjX2J3tjhG8zb6u0qHn";

        await InitDb();
        var client = Factory.CreateClient();
        var currentUserCredentials = await CreateTestUser();
        var secondUserCredentials =
            await CreateTestUser(new TestUserCredentials("secondUserUsername", "secondUserPassword"));

        await Login(secondUserCredentials.Username, secondUserCredentials.Password, client);
        await client.PatchAsJsonAsync($"Track/{trackId}/Like", "");
        await client.PostAsJsonAsync("Accounts/Logout", "");

        await Login(currentUserCredentials.Username, currentUserCredentials.Password, client);
        var res = await client.GetAsync("Library/FindLikedTracks?limit=1&offset=0");
        res.EnsureSuccessStatusCode();
        var findLikedTrackResultDto =
            JsonConvert.DeserializeObject<FindLikedTracksResultDto>(await res.Content.ReadAsStringAsync());

        Assert.NotNull(findLikedTrackResultDto);
        Assert.Empty(findLikedTrackResultDto.Items);
    }
}