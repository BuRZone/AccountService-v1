using AccountService.API.Common;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

/// <summary>
/// Запрос для получения счета по идентификатору.
/// </summary>
/// <param name="id">Идентификатор счета для получения.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class GetAccountQuery(Guid id) : IRequest<MbResult<Account?>>
{
    public Guid Id { get; } = id;
} 