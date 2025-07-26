using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Filters;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by DI container")]
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        // Обработка бизнес-логики ошибок (400 Bad Request)
        if (context.Exception is InvalidOperationException)
        {
            logger.LogWarning(context.Exception, "Business logic error");
            
            var result = new
            {
                type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                title = "Ошибка бизнес-логики.",
                status = 400,
                errors = new Dictionary<string, string[]>
                {
                    ["Message"] = [context.Exception.Message]
                },
                traceId = context.HttpContext.TraceIdentifier
            };

            context.Result = new BadRequestObjectResult(result);
            context.ExceptionHandled = true;
            return;
        }

        // Обработка остальных ошибок (500 Internal Server Error)
        logger.LogError(context.Exception, "Unhandled exception occurred");

        var errorResult = new
        {
            type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            title = "An error occurred while processing your request.",
            status = 500,
            detail = "An unexpected error occurred. Please try again later.",
            traceId = context.HttpContext.TraceIdentifier
        };

        context.Result = new ObjectResult(errorResult)
        {
            StatusCode = 500
        };
    }
} 