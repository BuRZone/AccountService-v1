using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class TransferCommand(TransferRequest request) : IRequest<(Transaction DebitTransaction, Transaction CreditTransaction)>
{
    public TransferRequest Request { get; } = request;
} 