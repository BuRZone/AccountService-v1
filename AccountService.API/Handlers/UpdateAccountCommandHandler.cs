using AccountService.API.Commands;
using AccountService.API.Common;
using AccountService.API.Models;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class UpdateAccountCommandHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<UpdateAccountCommand, MbResult<Account>>
{
    public async Task<MbResult<Account>> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var existingAccount = await accountStorageService.GetByIdAsync(request.Id, cancellationToken);
        if (existingAccount == null)
            return MbResult<Account>.Failure(new MbError("AccountNotFound", "Account not found."));

        existingAccount.OwnerId = request.Request.OwnerId;
        existingAccount.Type = request.Request.Type;
        existingAccount.Currency = request.Request.Currency;
        existingAccount.Balance = request.Request.Balance;
        existingAccount.InterestRate = request.Request.InterestRate;
        existingAccount.OpeningDate = request.Request.OpeningDate;
        existingAccount.ClosingDate = request.Request.ClosingDate;

        var updatedAccount = await accountStorageService.UpdateAsync(existingAccount, cancellationToken);
        return MbResult<Account>.Success(updatedAccount);
    }
} 