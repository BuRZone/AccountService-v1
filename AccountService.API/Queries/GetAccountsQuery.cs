using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class GetAccountsQuery(Guid? ownerId = null) : IRequest<List<Account>>
{
    public Guid? OwnerId { get; } = ownerId;
} 