using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class CreateAccountCommand(CreateAccountRequest request) : IRequest<Account>
{
    public CreateAccountRequest Request { get; } = request;
} 