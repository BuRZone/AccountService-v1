namespace AccountService.API.Services;

public class ClientVerificationService : IClientVerificationService
{
    private readonly HashSet<Guid> _existingClients =
    [
        Guid.Parse("11111111-1111-1111-1111-111111111111"), // Иван
        Guid.Parse("22222222-2222-2222-2222-222222222222"), // Анна
        Guid.Parse("33333333-3333-3333-3333-333333333333")  // Алексей
    ];

    public Task<bool> ClientExistsAsync(Guid ownerId, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(_existingClients.Contains(ownerId));
    }
} 