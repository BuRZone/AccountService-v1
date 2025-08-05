using AccountService.API.Common;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

/// <summary>
/// Команда для удаления счета.
/// </summary>
/// <param name="id">Идентификатор счета для удаления.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class DeleteAccountCommand(Guid id) : IRequest<MbResult<bool>>
{
    public Guid Id { get; } = id;
} 