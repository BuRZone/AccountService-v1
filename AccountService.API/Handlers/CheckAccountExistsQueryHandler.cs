using AccountService.API.Common;
using AccountService.API.Queries;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class CheckAccountExistsQueryHandler(IAccountStorageService accountStorageService) : IRequestHandler<CheckAccountExistsQuery, MbResult<bool>>
{
    public async Task<MbResult<bool>> Handle(CheckAccountExistsQuery request, CancellationToken cancellationToken)
    {
        var exists = await accountStorageService.ExistsAsync(request.Id, cancellationToken);
        return MbResult<bool>.Success(exists);
    }
} 