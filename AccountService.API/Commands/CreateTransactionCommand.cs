using AccountService.API.Common;
using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

/// <summary>
/// Команда для создания новой транзакции.
/// </summary>
/// <param name="request">Запрос, содержащий детали для новой транзакции.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class CreateTransactionCommand(CreateTransactionRequest request) : IRequest<MbResult<Transaction>>
{
    public CreateTransactionRequest Request { get; } = request;
} 