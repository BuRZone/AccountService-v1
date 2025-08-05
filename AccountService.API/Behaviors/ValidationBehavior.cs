using AccountService.API.Common;
using FluentValidation;
using MediatR;

namespace AccountService.API.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, MbResult<TResponse>>
    where TRequest : IRequest<MbResult<TResponse>>
{
    public async Task<MbResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<MbResult<TResponse>> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .GroupBy(f => f.PropertyName) 
                .Select(g => g.First())       
                .ToList();

            if (failures.Count != 0)
            {
                var errorDetails = failures.ToDictionary(
                    f => f.PropertyName,
                    f => new string[] { f.ErrorMessage }
                );
                var mbError = new MbError("ValidationError", "One or more validation errors occurred.", errorDetails);
                return MbResult<TResponse>.Failure(mbError);
            }
        }

        return await next();
    }
} 