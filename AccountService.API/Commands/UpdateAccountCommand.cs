using AccountService.API.Common;
using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

/// <summary>
/// Команда для обновления существующего счета.
/// </summary>
/// <param name="id">Идентификатор счета для обновления.</param>
/// <param name="request">Запрос, содержащий обновленные детали счета.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class UpdateAccountCommand(Guid id, UpdateAccountRequest request) : IRequest<MbResult<Account>>
{
    public Guid Id { get; } = id;
    public UpdateAccountRequest Request { get; } = request;
} 