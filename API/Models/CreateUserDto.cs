using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CreateUserDto
{
    public CreateUserDto(string username, string password, string data)
    {
        Username = username;
        Password = password;
        Data = data;
    }

    public string Username { get; }
    public string Password { get; }
    public string Data { get; }
}