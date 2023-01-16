using System.Net.Http.Json;
using Api.Models;
using SystemTests.Models;

namespace SystemTests;

public class TestBase
{
    protected readonly CustomWebApplicationFactory<Program> Factory;

    public TestBase(CustomWebApplicationFactory<Program> applicationFactory)
    {
        Factory = applicationFactory;
    }

    public async Task<TestUserCredentials> CreateTestUser()
    {
        var credentials = new TestUserCredentials(
            "testUsername",
            "testPassword"
        );
        var client = Factory.CreateClient();
        var newUser = new CreateUserDto(
            credentials.Username,
            credentials.Password,
            "{}"
        );
        await client.PostAsJsonAsync("/Accounts/Register", newUser);
        return credentials;
    }
}