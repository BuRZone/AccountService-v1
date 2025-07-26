using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Attributes;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used for validation")]
public class CurrencyValidationAttribute : ValidationAttribute
{
    private static readonly string[] SupportedCurrencies = ["USD", "EUR", "RUB", "GBP", "JPY", "CNY"];

    public override bool IsValid(object? value)
    {
        if (value == null)
            return true; // Null значения обрабатываются отдельно через Required

        if (value is not string currency)
            return false;

        // Проверяем, что это не пустая строка или пробелы
        if (string.IsNullOrWhiteSpace(currency))
            return false;

        // Проверяем длину (ISO 4217 = 3 символа)
        if (currency.Length != 3)
            return false;

        // Проверяем, что это поддерживаемая валюта
        return SupportedCurrencies.Contains(currency.ToUpperInvariant());
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} должен быть валидным кодом валюты ISO 4217 (3 символа). Поддерживаемые валюты: {string.Join(", ", SupportedCurrencies)}";
    }
} 