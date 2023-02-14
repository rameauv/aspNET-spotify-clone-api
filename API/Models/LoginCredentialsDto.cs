namespace Api.Models;

public record LoginCredentialsDto(string Username, string Password)
{
    public string Username { get; } = Username;
    public string Password { get; } = Password;
}