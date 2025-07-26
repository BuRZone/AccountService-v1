using AccountService.API.Models;
using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class GetAccountQueryHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<GetAccountQuery, Account?>
{
    public async Task<Account?> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        return await accountStorageService.GetByIdAsync(request.Id, cancellationToken);
    }
} 