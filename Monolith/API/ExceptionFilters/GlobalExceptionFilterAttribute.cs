using Api.Controllers.Shared.Error;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.ExceptionFilters;

public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly ILogger<GlobalExceptionFilterAttribute> _logger;

    public GlobalExceptionFilterAttribute(ILogger<GlobalExceptionFilterAttribute> logger)
    {
        _logger = logger;
    }
    
    public override void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "uncaught exception");
        var status = 500;
        var error = new ErrorDto(
            "Internal server error",
            status,
            ""
        );
        var errors = new ErrorsDto(new[] { error });
        context.Result = new ObjectResult(errors)
        {
            StatusCode = status
        };
        context.HttpContext.Response.Headers.ContentType = "application/problem+json";
    }
}