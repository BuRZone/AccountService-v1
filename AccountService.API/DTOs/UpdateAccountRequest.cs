using AccountService.API.Attributes;
using AccountService.API.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace AccountService.API.DTOs;

/// <summary>
/// Представляет запрос на обновление существующего счета.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used for serialization")]
public class UpdateAccountRequest
{
    /// <summary>
    /// Уникальный идентификатор владельца счета. Обязательно.
    /// </summary>
    [Required]
    public Guid OwnerId { get; set; }
    /// <summary>
    /// Тип счета (например, Checking, Deposit, Credit). Обязательно.
    /// </summary>
    [Required]
    public AccountType Type { get; set; }
    /// <summary>
    /// Валюта счета в формате ISO 4217 (например, USD, EUR, RUB). Обязательно.
    /// </summary>
    [Required]
    public string Currency { get; set; } = string.Empty;
    /// <summary>
    /// Текущий баланс счета. Обязательно.
    /// </summary>
    [Required]
    public decimal Balance { get; set; }
    /// <summary>
    /// Процентная ставка, применяемая к счету (опционально, для счетов Deposit и Credit).
    /// </summary>
    public decimal? InterestRate { get; set; }
    /// <summary>
    /// Дата и время открытия счета. Обязательно.
    /// </summary>
    [Required]
    public DateTime OpeningDate { get; set; }
    /// <summary>
    /// Дата и время закрытия счета (опционально).
    /// </summary>
    public DateTime? ClosingDate { get; set; }
}