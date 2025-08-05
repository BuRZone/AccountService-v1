using AccountService.API.Attributes;
using System.ComponentModel.DataAnnotations;
using AccountService.API.Models;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.DTOs;

/// <summary>
/// Представляет запрос на перевод средств между двумя счетами.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used for serialization")]
public class TransferRequest
{
    /// <summary>
    /// Уникальный идентификатор исходного счета (От).
    /// </summary>
    [Required]
    public Guid FromAccountId { get; set; }
    /// <summary>
    /// Уникальный идентификатор целевого счета (К).
    /// </summary>
    [Required]
    public Guid ToAccountId { get; set; }
    /// <summary>
    /// Сумма для перевода.
    /// </summary>
    [Required]
    public decimal Amount { get; set; }
    /// <summary>
    /// Валюта перевода в формате ISO 4217 (например, USD, EUR, RUB).
    /// </summary>
    [Required]
    public string Currency { get; set; } = string.Empty;
    /// <summary>
    /// Описание для перевода.
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;
}