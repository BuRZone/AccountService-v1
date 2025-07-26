using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccountService.API.Filters;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
public class ValidationExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is not ValidationException validationException) return;
        var errors = validationException.Errors.Select(error => new
        {
            Property = error.PropertyName,
            Message = error.ErrorMessage
        });

        var result = new BadRequestObjectResult(new
        {
            Message = "Validation failed",
            Errors = errors
        });

        context.Result = result;
        context.ExceptionHandled = true;
    }
} 