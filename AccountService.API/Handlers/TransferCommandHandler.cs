using AccountService.API.Commands;
using AccountService.API.Common;
using AccountService.API.Models;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class TransferCommandHandler(IAccountStorageService accountStorageService)
    : IRequestHandler<TransferCommand, MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>>
{
    public async Task<MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var fromAccount = await accountStorageService.GetByIdAsync(request.Request.FromAccountId, cancellationToken);
        var toAccount = await accountStorageService.GetByIdAsync(request.Request.ToAccountId, cancellationToken);

        if (fromAccount == null)
            return MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>.Failure(new MbError("AccountNotFound", "Счет отправителя не найден"));
        if (toAccount == null)
            return MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>.Failure(new MbError("AccountNotFound", "Счет получателя не найден"));
        if (fromAccount.Balance < request.Request.Amount)
            return MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>.Failure(new MbError("InsufficientFunds", "Недостаточно средств на счете"));
        if (fromAccount.OwnerId != toAccount.OwnerId)
            return MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>.Failure(new MbError("OwnerMismatch", "Счета должны принадлежать одному клиенту"));
        if (fromAccount.Currency != toAccount.Currency)
            return MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>.Failure(new MbError("CurrencyMismatch", "Валюты счетов должны совпадать"));

        Transaction debitTransaction = new()
        {
            Id = Guid.NewGuid(),
            AccountId = request.Request.FromAccountId,
            CounterpartyAccountId = request.Request.ToAccountId,
            Amount = request.Request.Amount,
            Currency = request.Request.Currency,
            Type = TransactionType.Debit,
            Description = $"Перевод на счет {request.Request.ToAccountId}: {request.Request.Description}",
            DateTime = DateTime.UtcNow
        };

        Transaction creditTransaction = new()
        {
            Id = Guid.NewGuid(),
            AccountId = request.Request.ToAccountId,
            CounterpartyAccountId = request.Request.FromAccountId,
            Amount = request.Request.Amount,
            Currency = request.Request.Currency,
            Type = TransactionType.Credit,
            Description = $"Перевод со счета {request.Request.FromAccountId}: {request.Request.Description}",
            DateTime = DateTime.UtcNow
        };

        fromAccount.Balance -= request.Request.Amount;
        toAccount.Balance += request.Request.Amount;

        fromAccount.Transactions.Add(debitTransaction);
        toAccount.Transactions.Add(creditTransaction);

        await accountStorageService.UpdateAsync(fromAccount, cancellationToken);
        await accountStorageService.UpdateAsync(toAccount, cancellationToken);

        return MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>.Success((debitTransaction, creditTransaction));
    }
} 