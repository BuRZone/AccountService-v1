using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class DeleteAccountCommand(Guid id) : IRequest<bool>
{
    public Guid Id { get; } = id;
} 