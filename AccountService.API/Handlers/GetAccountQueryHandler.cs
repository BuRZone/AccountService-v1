using AccountService.API.Common;
using AccountService.API.Models;
using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class GetAccountQueryHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<GetAccountQuery, MbResult<Account?>>
{
    public async Task<MbResult<Account?>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetByIdAsync(request.Id, cancellationToken);
        if (account == null)
            return MbResult<Account?>.Failure(new MbError("AccountNotFound", "Account not found."));

        return MbResult<Account?>.Success(account);
    }
} 