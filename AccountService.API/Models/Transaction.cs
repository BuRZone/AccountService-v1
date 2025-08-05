using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Models;

/// <summary>
/// Представляет финансовую транзакцию.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used for serialization")]
public class Transaction
{
    /// <summary>
    /// Уникальный идентификатор транзакции.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Идентификатор счета, связанного с этой транзакцией.
    /// </summary>
    public Guid AccountId { get; set; }
    /// <summary>
    /// Идентификатор счета контрагента (опционально).
    /// </summary>
    public Guid? CounterpartyAccountId { get; set; }
    /// <summary>
    /// Сумма транзакции.
    /// </summary>
    public decimal Amount { get; set; }
    /// <summary>
    /// Валюта транзакции в формате ISO 4217 (например, USD, EUR, RUB).
    /// </summary>
    public string Currency { get; set; } = string.Empty;
    /// <summary>
    /// Тип транзакции (Credit или Debit).
    /// </summary>
    public TransactionType Type { get; set; }
    /// <summary>
    /// Описание транзакции.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Дата и время, когда произошла транзакция.
    /// </summary>
    public DateTime DateTime { get; set; }
}

/// <summary>
/// Определяет возможные типы финансовых транзакций.
/// </summary>
public enum TransactionType
{
    /// <summary>
    /// Кредитная транзакция (деньги поступают на счет).
    /// </summary>
    Credit = 0,
    /// <summary>
    /// Дебетовая транзакция (деньги уходят со счета).
    /// </summary>
    Debit = 1
} 