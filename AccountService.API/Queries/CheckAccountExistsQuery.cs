using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class CheckAccountExistsQuery(Guid id) : IRequest<bool>
{
    public Guid Id { get; } = id;
} 