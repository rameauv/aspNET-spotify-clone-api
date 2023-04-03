using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Shared.Error;

public class ErrorResult : ObjectResult
{
    public ErrorResult(ErrorDto error) : base(new ErrorsDto(error))
    {
        base.StatusCode = error.Status;
    }
}