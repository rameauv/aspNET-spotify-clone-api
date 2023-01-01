namespace Api.Models;

public record LoginCredentialsDto(string Username, string Password)
{
    public string Username { get; set; } = Username;
    public string Password { get; set; } = Password;
}