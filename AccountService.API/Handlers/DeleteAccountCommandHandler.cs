using AccountService.API.Commands;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class DeleteAccountCommandHandler(IAccountStorageService accountStorageService) : IRequestHandler<DeleteAccountCommand, bool>
{
    public async Task<bool> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var exists = await accountStorageService.ExistsAsync(request.Id, cancellationToken);
        if (!exists)
            return false;

        await accountStorageService.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
} 