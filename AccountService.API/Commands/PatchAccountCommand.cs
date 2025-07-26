using AccountService.API.DTOs;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Commands;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class PatchAccountCommand(Guid id, PatchAccountRequest request) : IRequest<Account>
{
    public Guid Id { get; } = id;
    public PatchAccountRequest Request { get; } = request;
} 