using AccountService.API.Attributes;
using AccountService.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.DTOs;

/// <summary>
/// Запрос на создание новой транзакции
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class CreateTransactionRequest
{
    /// <summary>
    /// ID счета для транзакции (GUID)
    /// </summary>
    [Required]
    public Guid AccountId { get; set; }

    /// <summary>
    /// ID счета контрагента (GUID, опционально). Для внешних переводов
    /// </summary>
    public Guid? CounterpartyAccountId { get; set; }

    /// <summary>
    /// Сумма транзакции (decimal, больше 0). Формат: 100.50 (точка как разделитель)
    /// </summary>
    [Range(typeof(decimal), "0.01", "999999999999999999.99", ErrorMessage = "Сумма должна быть от 0.01 до 999999999999999999.99")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Валюта транзакции (ISO 4217 код, 3 символа). Примеры: USD, EUR, RUB
    /// </summary>
    [Required]
    [CurrencyValidation]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Тип транзакции: 0 - Credit (пополнение), 1 - Debit (списание)
    /// </summary>
    [Required]
    [EnumDataType(typeof(TransactionType))]
    public TransactionType Type { get; set; }

    /// <summary>
    /// Описание транзакции (максимум 500 символов)
    /// </summary>
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;
}