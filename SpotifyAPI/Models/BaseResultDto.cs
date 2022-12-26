using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SpotifyApi.Models;

public class BaseResultDto<T>
{
    public T? Result { get; set; }
    public ErrorDto? Error { get; set; }
}

public class SuccessResultDto<T> : BaseResultDto<T>
{
    public SuccessResultDto(T result)
    {
        Result = result;
    }

    [Required]
    public new T Result { get; set; }
    private new ErrorDto? Error => null;
}

public class ErrorResultDto<T> : BaseResultDto<T>
{
    private new T? Result => default(T);

    public ErrorResultDto(ErrorDto error)
    {
        Error = error;
    }

    [Required]
    public new ErrorDto Error { get; set; }
}

public class ErrorDto
{
    public ErrorDto(HttpStatusCode code, string message)
    {
        Code = code;
        Message = message;
    }

    public HttpStatusCode Code { get; set; }
    public string Message { get; set; }
}