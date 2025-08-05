using AccountService.API.Common;
using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class GetAccountStatementQueryHandler(IAccountStorageService accountStorageService) : IRequestHandler<GetAccountStatementQuery, MbResult<AccountStatement>>
{
    public async Task<MbResult<AccountStatement>> Handle(GetAccountStatementQuery request, CancellationToken cancellationToken)
    {
        var account = await accountStorageService.GetByIdAsync(request.AccountId, cancellationToken);
        if (account == null)
        {
            return MbResult<AccountStatement>.Failure(new MbError("AccountNotFound", "Счет не найден."));
        }

        var transactions = account.Transactions
            .Where(t => t.DateTime >= request.StartDate && t.DateTime <= request.EndDate)
            .OrderBy(t => t.DateTime)
            .ToList();

        // Пересчитываем начальный баланс, отменяя транзакции в пределах периода от текущего баланса
        decimal tempBalance = account.Balance;
        foreach (var transaction in account.Transactions.OrderByDescending(t => t.DateTime))
        {
            if (transaction.DateTime > request.EndDate)
            { 
                // Если транзакция после даты окончания, она уже включена в текущий баланс, поэтому убираем её эффект
                if (transaction.Type == Models.TransactionType.Credit) tempBalance -= transaction.Amount;
                else tempBalance += transaction.Amount;
            }
            else if (transaction.DateTime < request.StartDate)
            {
                // Это транзакции до даты начала. Их эффект является частью начального баланса.
                break; // Останавливаемся, когда уходим до даты начала
            }
        }

        AccountStatement statement = new()
        {
            AccountId = request.AccountId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            OpeningBalance = tempBalance,
            ClosingBalance = account.Balance,
            Transactions = transactions
        };

        return MbResult<AccountStatement>.Success(statement);
    }
}