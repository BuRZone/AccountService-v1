using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class CheckAccountExistsQueryHandler(IAccountStorageService accountStorageService) : IRequestHandler<CheckAccountExistsQuery, bool>
{
    public async Task<bool> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
    {
        return await accountStorageService.ExistsAsync(request.Id, cancellationToken);
    }
} 