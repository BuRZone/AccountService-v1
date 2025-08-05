using AccountService.API.Common;
using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

/// <summary>
/// Команда для создания нового счета.
/// </summary>
/// <param name="request">Запрос, содержащий детали для нового счета.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class CreateAccountCommand(CreateAccountRequest request) : IRequest<MbResult<Account>>
{
    public CreateAccountRequest Request { get; } = request;
} 