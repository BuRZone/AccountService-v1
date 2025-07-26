using AccountService.API.Models;

namespace AccountService.API.Services;

public interface IAccountStorageService
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Account>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<Account>> GetByOwnerIdAsync(Guid ownerId, CancellationToken cancellationToken = default);
    Task<Account> CreateAsync(Account account, CancellationToken cancellationToken = default);
    Task<Account> UpdateAsync(Account account, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
} 