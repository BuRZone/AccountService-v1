using AccountService.API.Common;
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
        if (context.Exception is InvalidOperationException || context.Exception is ArgumentException)
        {
            logger.LogWarning(context.Exception, "Business logic error or invalid argument");
            
            var mbError = new MbError("BusinessLogicError", context.Exception.Message);
            var result = MbResult<object>.Failure(mbError);

            context.Result = new BadRequestObjectResult(result);
            context.ExceptionHandled = true;
            return;
        }

        logger.LogError(context.Exception, "Unhandled exception occurred");

        var internalServerError = new MbError("InternalServerError", "An unexpected error occurred. Please try again later.");
        var errorResult = MbResult<object>.Failure(internalServerError);

        context.Result = new ObjectResult(errorResult)
        {
            StatusCode = 500
        };
        context.ExceptionHandled = true;
    }
} 