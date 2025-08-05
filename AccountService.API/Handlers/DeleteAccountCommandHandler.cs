using AccountService.API.Commands;
using AccountService.API.Common;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class DeleteAccountCommandHandler(IAccountStorageService accountStorageService) : IRequestHandler<DeleteAccountCommand, MbResult<bool>>
{
    public async Task<MbResult<bool>> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var exists = await accountStorageService.ExistsAsync(request.Id, cancellationToken);
        if (!exists)
            return MbResult<bool>.Failure(new MbError("AccountNotFound", "Account not found."));

        await accountStorageService.DeleteAsync(request.Id, cancellationToken);
        return MbResult<bool>.Success(true);
    }
} 