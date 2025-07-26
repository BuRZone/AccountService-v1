using AccountService.API.Commands;
using AccountService.API.Models;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class PatchAccountCommandHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<PatchAccountCommand, Account>
{
    public async Task<Account> Handle(PatchAccountCommand request, CancellationToken cancellationToken)
    {
        Account? existingAccount = await accountStorageService.GetByIdAsync(request.Id, cancellationToken);
        if (existingAccount == null)
            throw new ArgumentException("Account not found");

        if (request.Request.OwnerId.HasValue)
            existingAccount.OwnerId = request.Request.OwnerId.Value;
            
        if (request.Request.Type.HasValue)
            existingAccount.Type = request.Request.Type.Value;
            
        if (!string.IsNullOrEmpty(request.Request.Currency))
            existingAccount.Currency = request.Request.Currency;
            
        if (request.Request.Balance.HasValue)
            existingAccount.Balance = request.Request.Balance.Value;
            
        if (request.Request.InterestRate.HasValue)
            existingAccount.InterestRate = request.Request.InterestRate.Value;
            
        if (request.Request.OpeningDate.HasValue)
            existingAccount.OpeningDate = request.Request.OpeningDate.Value;
            
        if (request.Request.ClosingDate.HasValue)
            existingAccount.ClosingDate = request.Request.ClosingDate.Value;

        return await accountStorageService.UpdateAsync(existingAccount, cancellationToken);
    }
} 