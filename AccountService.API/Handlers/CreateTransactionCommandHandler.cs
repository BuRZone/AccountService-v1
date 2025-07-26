using AccountService.API.Commands;
using AccountService.API.Models;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class CreateTransactionCommandHandler(IAccountStorageService accountStorageService) : IRequestHandler<CreateTransactionCommand, Transaction>
{
    public async Task<Transaction> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetByIdAsync(request.Request.AccountId, cancellationToken);
        if (account == null)
            throw new ArgumentException("Аккаунт не найден");

        
        if (request.Request.Type == TransactionType.Debit && account.Balance < request.Request.Amount)
            throw new InvalidOperationException("Недостаточно средств на счете");

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

        return transaction;
    }
} 