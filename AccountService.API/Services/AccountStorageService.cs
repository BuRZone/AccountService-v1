using AccountService.API.Models;

namespace AccountService.API.Services;

public class AccountStorageService : IAccountStorageService
{
    private readonly List<Account> _accounts = [];

    public Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var account = _accounts.FirstOrDefault(a => a.Id == id);
        return Task.FromResult(account);
    }

    public Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_accounts.ToList());
    }

    public Task<List<Account>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var accounts = _accounts.Where(a => a.OwnerId == ownerId).ToList();
        return Task.FromResult(accounts);
    }

    public Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        account.Id = Guid.NewGuid();
        account.OpeningDate = DateTime.UtcNow;
        _accounts.Add(account);
        return Task.FromResult(account);
    }

    public Task<Account> UpdateAsync(Account account, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var existingAccount = _accounts.FirstOrDefault(a => a.Id == account.Id);
        if (existingAccount == null)
            throw new ArgumentException("Account not found");

        var index = _accounts.IndexOf(existingAccount);
        _accounts[index] = account;
        return Task.FromResult(account);
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var account = _accounts.FirstOrDefault(a => a.Id == id);
        if (account != null)
            _accounts.Remove(account);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var exists = _accounts.Any(a => a.Id == id);
        return Task.FromResult(exists);
    }
} 