using AccountService.API.Attributes;
using AccountService.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.DTOs;

/// <summary>
/// Представляет запрос на создание новой транзакции.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used for serialization")]
public class CreateTransactionRequest
{
    /// <summary>
    /// Уникальный идентификатор счета, участвующего в транзакции. Обязательно.
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }
    /// <summary>
    /// Уникальный идентификатор счета контрагента (опционально).
    /// </summary>
    public Guid? CounterpartyAccountId { get; set; }
    /// <summary>
    /// Сумма транзакции. Обязательно.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }
    /// <summary>
    /// Валюта транзакции в формате ISO 4217 (например, USD, EUR, RUB). Обязательно.
    /// </summary>
    [Required]
    public string Currency { get; set; } = string.Empty;
    /// <summary>
    /// Тип транзакции (Credit или Debit). Обязательно.
    /// </summary>
    [Required]
    public TransactionType Type { get; set; }
    /// <summary>
    /// Описание транзакции. Обязательно.
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;
}