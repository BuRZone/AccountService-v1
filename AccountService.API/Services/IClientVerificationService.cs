namespace AccountService.API.Services;

public interface IClientVerificationService
{
    Task<bool> ClientExistsAsync(Guid ownerId, CancellationToken cancellationToken = default);
} 