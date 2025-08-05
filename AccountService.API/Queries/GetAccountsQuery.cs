using AccountService.API.Common;
using AccountService.API.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

/// <summary>
/// Запрос для получения списка счетов.
/// </summary>
/// <param name="ownerId">Опциональный идентификатор владельца для фильтрации счетов.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class GetAccountsQuery(Guid? ownerId = null) : IRequest<MbResult<List<Account>>>
{
    public Guid? OwnerId { get; } = ownerId;
} 