using System.Net;

namespace SpotifyApi.Models;

public class ResultDto<T>
{
    public T Result { get; set; }
    public ErrorDto Error { get; set; }
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