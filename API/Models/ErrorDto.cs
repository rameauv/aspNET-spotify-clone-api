using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class ErrorDto
{
    public ErrorDto(string title, int status, string details, string? type = null, string? instance = null)
    {
        Title = title;
        Status = status;
        Details = details;
        Type = type ?? "about:bank";
        Instance = instance ?? "about:bank";
    }

    [Required] public string Type { get; }
    [Required] public string Title { get; }
    [Required] public int Status { get; }
    [Required] public string Details { get; }
    [Required] public string Instance { get; }
}