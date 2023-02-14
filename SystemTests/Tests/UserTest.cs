using System.Net.Http.Json;
using Api.Models;
using Newtonsoft.Json;

namespace SystemTests.Tests;

/// <summary>
/// Test class for testing the User API.
/// </summary>
[Collection("system tests")]
public class UserTest: TestBase, IClassFixture<CustomWebApplicationFactory<Program>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserTest"/> class.
    /// </summary>
    /// <param name="applicationFactory">The application factory.</param>
    public UserTest(CustomWebApplicationFactory<Program> applicationFactory) : base(applicationFactory)
    {
    }

    /// <summary>
    /// Tests that the API properly return the current user
    /// </summary>
    [Fact]
    public async Task ShouldGetTheCurrentUser()
    {
        var client = Factory.CreateClient();
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var response = await client.GetAsync("User/CurrentUser");
        response.EnsureSuccessStatusCode();
        var serializedCurrentUserDto = await response.Content.ReadAsStringAsync();
        var currentUserDto = JsonConvert.DeserializeObject<CurrentUserDto>(serializedCurrentUserDto);

        Assert.NotNull(currentUserDto);
        Assert.NotNull(currentUserDto.Id);
    }

    /// <summary>
    /// Tests that the API properly set the profile name of the current user
    /// </summary>
    [Fact]
    public async Task ShouldSetTheCurrentUserProfileName()
    {
        var newProfileName = "new profile name";
        var client = Factory.CreateClient();
        await InitDb();
        var testUserCredentials = await CreateTestUser();
        await Login(testUserCredentials.Username, testUserCredentials.Password, client);

        var setNameRequestDto = new SetNameRequestDto(newProfileName);
        var setProfileNameResponse = await client.PatchAsJsonAsync("User/Name", setNameRequestDto);
        setProfileNameResponse.EnsureSuccessStatusCode();
        var currentUserResponse = await client.GetAsync("User/CurrentUser");
        currentUserResponse.EnsureSuccessStatusCode();
        var serializedCurrentUserDto = await currentUserResponse.Content.ReadAsStringAsync();
        var currentUserDto = JsonConvert.DeserializeObject<CurrentUserDto>(serializedCurrentUserDto);
        
        Assert.NotNull(currentUserDto);
        Assert.Equal(newProfileName, currentUserDto.Name);
    }
}