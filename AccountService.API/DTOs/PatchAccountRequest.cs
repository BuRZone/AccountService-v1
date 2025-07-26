using AccountService.API.Attributes;
using AccountService.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.DTOs;

/// <summary>
/// Запрос на частичное обновление счета
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class PatchAccountRequest
{
    /// <summary>
    /// ID владельца счета (GUID, опционально). Клиент должен существовать в системе
    /// </summary>
    public Guid? OwnerId { get; set; }

    /// <summary>
    /// Тип счета (опционально): 0 - Checking (текущий), 1 - Deposit (депозит), 2 - Credit (кредитный)
    /// </summary>
    [EnumDataType(typeof(AccountType))]
    public AccountType? Type { get; set; }

    /// <summary>
    /// Валюта счета (опционально, ISO 4217 код, 3 символа). Примеры: USD, EUR, RUB
    /// </summary>
    [CurrencyValidation]
    public string? Currency { get; set; }

    /// <summary>
    /// Баланс счета (опционально, decimal, от 0 до 999999999999999999.99). Формат: 1000.50 (точка как разделитель)
    /// </summary>
    [Range(typeof(decimal), "0", "999999999999999999.99", ErrorMessage = "Баланс должен быть от 0 до 999999999999999999.99")]
    public decimal? Balance { get; set; }

    /// <summary>
    /// Процентная ставка (опционально, decimal, от 0 до 100). Формат: 5.5 (точка как разделитель)
    /// </summary>
    [Range(typeof(decimal), "0", "100", ErrorMessage = "Процентная ставка должна быть от 0 до 100")]
    public decimal? InterestRate { get; set; }

    /// <summary>
    /// Дата открытия счета (опционально). Формат: "2024-01-15T10:30:00" (ISO 8601)
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime? OpeningDate { get; set; }

    /// <summary>
    /// Дата закрытия счета (опционально). Формат: "2024-12-31T23:59:59" (ISO 8601). Должна быть после даты открытия
    /// </summary>
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime? ClosingDate { get; set; }
}