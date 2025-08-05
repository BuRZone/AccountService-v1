using AccountService.API.Common;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Queries;

/// <summary>
/// Запрос для проверки существования счета по идентификатору.
/// </summary>
/// <param name="id">Идентификатор счета для проверки.</param>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Used by MediatR")]
public class CheckAccountExistsQuery(Guid id) : IRequest<MbResult<bool>>
{
    public Guid Id { get; } = id;
} 