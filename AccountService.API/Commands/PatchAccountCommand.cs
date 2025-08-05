using AccountService.API.Common;
using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

/// <summary>
/// Команда для частичного обновления счета.
/// </summary>
/// <param name="id">Идентификатор счета для обновления.</param>
/// <param name="request">Запрос, содержащий поля для обновления.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class PatchAccountCommand(Guid id, PatchAccountRequest request) : IRequest<MbResult<Account>>
{
    public Guid Id { get; } = id;
    public PatchAccountRequest Request { get; } = request;
} 