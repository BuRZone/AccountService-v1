using AccountService.API.Attributes;
using AccountService.API.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.DTOs;

/// <summary>
/// Запрос на создание нового счета
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class CreateAccountRequest
{
    /// <summary>
    /// ID владельца счета (GUID). Клиент должен существовать в системе
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
    /// Процентная ставка (decimal, от 0 до 100). Формат: 5.5 (точка как разделитель). Обязательно для Deposit и Credit счетов
    /// </summary>
    [Range(typeof(decimal), "0", "100", ErrorMessage = "Процентная ставка должна быть от 0 до 100")]
    [DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
    public decimal? InterestRate { get; set; }
}