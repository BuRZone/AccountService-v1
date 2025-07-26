using AccountService.API.Attributes;
using AccountService.API.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace AccountService.API.DTOs;

/// <summary>
/// Запрос на полное обновление счета
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class UpdateAccountRequest
{
    /// <summary>
    /// ID владельца счета (GUID). Пример: "11111111-1111-1111-1111-111111111111"
    /// </summary>
    [Required]
    public Guid OwnerId { get; set; }

    /// <summary>
    /// Тип счета: 0 - Checking (текущий), 1 - Deposit (депозит), 2 - Credit (кредитный)
    /// </summary>
    [Required]
    [EnumDataType(typeof(AccountType))]
    public AccountType Type { get; set; }

    /// <summary>
    /// Валюта счета (ISO 4217 код, 3 символа). Примеры: USD, EUR, RUB
    /// </summary>
    [Required]
    [CurrencyValidation]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Баланс счета (decimal, от 0 до 999999999999999999.99). Формат: 1000.50 (точка как разделитель). Пример: 1000.50
    /// </summary>
    [Range(typeof(decimal), "0", "999999999999999999.99", ErrorMessage = "Баланс должен быть от 0 до 999999999999999999.99")]
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    [SwaggerSchema(Description = "Баланс счета")]
    public decimal Balance { get; set; }

    /// <summary>
    /// Процентная ставка (decimal, от 0 до 100). Формат: 5.5 (точка как разделитель). Обязательно для Deposit и Credit счетов. Пример: 5.5
    /// </summary>
    [Range(typeof(decimal), "0", "100", ErrorMessage = "Процентная ставка должна быть от 0 до 100")]
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    public decimal? InterestRate { get; set; }

    /// <summary>
    /// Дата открытия счета. Формат: "2024-01-15T10:30:00" (ISO 8601). Пример: "2024-01-15T10:30:00"
    /// </summary>
    [Required]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime OpeningDate { get; set; }

    /// <summary>
    /// Дата закрытия счета (опционально). Формат: "2024-12-31T23:59:59" (ISO 8601). Должна быть после даты открытия. Пример: "2024-12-31T23:59:59"
    /// </summary>
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime? ClosingDate { get; set; }
}