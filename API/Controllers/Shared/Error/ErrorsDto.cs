using System.ComponentModel.DataAnnotations;

namespace Api.Controllers.Shared.Error;

public class ErrorsDto
{
    public ErrorsDto(IEnumerable<ErrorDto> errors)
    {
        Errors = errors;
    }

    public ErrorsDto(ErrorDto error)
    {
        Errors = new[] { error };
    }

    [Required] public IEnumerable<ErrorDto> Errors { get; }
}