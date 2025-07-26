using AccountService.API.Commands;
using AccountService.API.Models;
using AccountService.API.Services;
using MediatR;

namespace AccountService.API.Handlers;

public class CreateAccountCommandHandler(
    IAccountStorageService accountStorageService) : IRequestHandler<CreateAccountCommand, Account>
{
    public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        Account account = new()
        {
            OwnerId = request.Request.OwnerId,
            Type = request.Request.Type,
            Currency = request.Request.Currency,
            Balance = 0,
            InterestRate = request.Request.InterestRate,
            OpeningDate = DateTime.UtcNow
        };

        return await accountStorageService.CreateAsync(account, cancellationToken);
    }
} 