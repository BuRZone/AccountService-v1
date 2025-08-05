using AccountService.API.Commands;
using AccountService.API.Common;
using AccountService.API.Models;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class CreateTransactionCommandHandler(IAccountStorageService accountStorageService) : IRequestHandler<CreateTransactionCommand, MbResult<Transaction>>
{
    public async Task<MbResult<Transaction>> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetByIdAsync(request.Request.AccountId, cancellationToken);
        if (account == null)
            return MbResult<Transaction>.Failure(new MbError("AccountNotFound", "Аккаунт не найден"));

        if (request.Request.Type == TransactionType.Debit && account.Balance < request.Request.Amount)
            return MbResult<Transaction>.Failure(new MbError("InsufficientFunds", "Недостаточно средств на счете"));

        Transaction transaction = new()
        {
            Id = Guid.NewGuid(),
            AccountId = request.Request.AccountId,
            CounterpartyAccountId = request.Request.CounterpartyAccountId,
            Amount = request.Request.Amount,
            Currency = request.Request.Currency,
            Type = request.Request.Type,
            Description = request.Request.Description,
            DateTime = DateTime.UtcNow
        };

        if (request.Request.Type == TransactionType.Credit)
            account.Balance += request.Request.Amount;
        else
            account.Balance -= request.Request.Amount;

        account.Transactions.Add(transaction);
        await accountStorageService.UpdateAsync(account, cancellationToken);

        return MbResult<Transaction>.Success(transaction);
    }
} 