using AccountService.API.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.DTOs;

/// <summary>
/// Запрос на перевод средств между счетами
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
public class TransferRequest
{
    /// <summary>
    /// ID счета отправителя (GUID)
    /// </summary>
    [Required]
    public Guid FromAccountId { get; set; }

    /// <summary>
    /// ID счета получателя (GUID). Должен отличаться от FromAccountId
    /// </summary>
    [Required]
    public Guid ToAccountId { get; set; }

    /// <summary>
    /// Сумма перевода (decimal, больше 0). Формат: 100.50 (точка как разделитель)
    /// </summary>
    [Range(typeof(decimal), "0.01", "999999999999999999.99", ErrorMessage = "Сумма должна быть от 0.01 до 999999999999999999.99")]
    public decimal Amount { get; set; }

    /// <summary>
    /// Валюта перевода (ISO 4217 код, 3 символа). Примеры: USD, EUR, RUB
    /// </summary>
    [Required]
    [CurrencyValidation]
    public string Currency { get; set; } = string.Empty;

    /// <summary>
    /// Описание перевода (максимум 500 символов)
    /// </summary>
    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;
}