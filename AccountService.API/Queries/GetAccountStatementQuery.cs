using AccountService.API.Common;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

/// <summary>
/// Запрос для получения выписки по счету за указанный период.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used for MediatR")]
public record GetAccountStatementQuery(Guid AccountId, DateTime StartDate, DateTime EndDate) : IRequest<MbResult<AccountStatement>>;

/// <summary>
/// Представляет выписку по счету, включая детали счета и соответствующие транзакции.
/// </summary>
public class AccountStatement
{
    /// <summary>
    /// Идентификатор счета, для которого формируется выписка.
    /// </summary>
    public Guid AccountId { get; set; }

    /// <summary>
    /// Дата начала периода выписки.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// Дата окончания периода выписки.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Начальный баланс счета на начало периода.
    /// </summary>
    public decimal OpeningBalance { get; set; }

    /// <summary>
    /// Конечный баланс счета на конец периода.
    /// </summary>
    public decimal ClosingBalance { get; set; }

    /// <summary>
    /// Список транзакций, произошедших в течение периода выписки.
    /// </summary>
    public List<Transaction> Transactions { get; set; } = [];
}