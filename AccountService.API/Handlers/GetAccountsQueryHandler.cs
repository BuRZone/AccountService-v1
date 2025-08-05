using AccountService.API.Common;
using AccountService.API.Models;
using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class GetAccountsQueryHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<GetAccountsQuery, MbResult<List<Account>>>
{
    public async Task<MbResult<List<Account>>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        List<Account> accounts;
        if (request.OwnerId.HasValue)
            accounts = await accountStorageService.GetByOwnerIdAsync(request.OwnerId.Value, cancellationToken);
        else
            accounts = await accountStorageService.GetAllAsync(cancellationToken);

        return MbResult<List<Account>>.Success(accounts);
    }
} 