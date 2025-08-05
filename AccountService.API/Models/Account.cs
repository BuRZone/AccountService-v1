using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Models;

/// <summary>
/// Представляет банковский счет.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used for serialization")]
public class Account
{
    /// <summary>
    /// Уникальный идентификатор счета.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Уникальный идентификатор владельца счета.
    /// </summary>
    public Guid OwnerId { get; set; }
    /// <summary>
    /// Тип счета (например, Checking, Deposit, Credit).
    /// </summary>
    public AccountType Type { get; set; }
    /// <summary>
    /// Валюта счета в формате ISO 4217 (например, USD, EUR, RUB).
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    /// <summary>
    /// Текущий баланс счета.
    /// </summary>
    public decimal Balance { get; set; }
    /// <summary>
    /// Процентная ставка, применяемая к счету (опционально, для счетов Deposit и Credit).
    /// </summary>
    public decimal? InterestRate { get; set; }
    /// <summary>
    /// Дата и время открытия счета.
    /// </summary>
    public DateTime OpeningDate { get; set; }
    /// <summary>
    /// Дата и время закрытия счета (опционально).
    /// </summary>
    public DateTime? ClosingDate { get; set; }
    /// <summary>
    /// Коллекция транзакций, связанных с этим счетом.
    /// </summary>
    public List<Transaction> Transactions { get; set; } = [];
}

/// <summary>
/// Определяет возможные типы банковских счетов.
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Текущий счет.
    /// </summary>
    Checking = 0,
    /// <summary>
    /// Депозитный счет.
    /// </summary>
    Deposit = 1,
    /// <summary>
    /// Кредитный счет.
    /// </summary>
    Credit = 2
}