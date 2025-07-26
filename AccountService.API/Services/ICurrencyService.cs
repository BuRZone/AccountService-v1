namespace AccountService.API.Services;

public interface ICurrencyService
{
    Task<bool> IsSupportedCurrencyAsync(string currency, CancellationToken cancellationToken = default);
} 