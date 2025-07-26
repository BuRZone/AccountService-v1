namespace AccountService.API.Services;

public class CurrencyService : ICurrencyService
{
    private readonly HashSet<string> _supportedCurrencies =
    [
        "RUB", "USD", "EUR", "GBP", "CNY"
    ];

    public Task<bool> IsSupportedCurrencyAsync(string currency, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_supportedCurrencies.Contains(currency.ToUpper()));
    }
} 