using AccountService.API.Common;
using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

/// <summary>
/// Команда для перевода средств между счетами.
/// </summary>
/// <param name="request">Запрос, содержащий детали для перевода.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class TransferCommand(TransferRequest request) : IRequest<MbResult<(Transaction DebitTransaction, Transaction CreditTransaction)>>
{
    public TransferRequest Request { get; } = request;
} 