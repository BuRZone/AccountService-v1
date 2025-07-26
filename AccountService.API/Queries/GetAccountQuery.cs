using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class GetAccountQuery(Guid id) : IRequest<Account?>
{
    public Guid Id { get; } = id;
} 