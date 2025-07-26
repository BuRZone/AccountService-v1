using AccountService.API.Models;
using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class GetAccountsQueryHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<GetAccountsQuery, List<Account>>
{
    public async Task<List<Account>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        if (request.OwnerId.HasValue)
            return await accountStorageService.GetByOwnerIdAsync(request.OwnerId.Value, cancellationToken);

        return await accountStorageService.GetAllAsync(cancellationToken);
    }
} 