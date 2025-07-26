using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class CreateTransactionCommand(CreateTransactionRequest request) : IRequest<Transaction>
{
    public CreateTransactionRequest Request { get; } = request;
} 