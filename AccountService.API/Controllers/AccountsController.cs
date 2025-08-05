using AccountService.API.Commands;
using AccountService.API.Common;
using AccountService.API.DTOs;
using AccountService.API.Models;
using AccountService.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить список всех счетов
    /// </summary>
    /// <param name="ownerId">Идентификатор владельца для фильтрации (опционально)</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список счетов</returns>
    /// <response code="200">Список счетов успешно получен</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet]
    [ProducesResponseType(typeof(MbResult<List<Account>>), 200)]
    [ProducesResponseType(typeof(MbResult<List<Account>>), 500)]
    public async Task<ActionResult<MbResult<List<Account>>>> GetAccounts([FromQuery] Guid? ownerId, CancellationToken cancellationToken)
    {
        GetAccountsQuery query = new(ownerId);
        var result = await mediator.Send(query, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }
        
        // Этот случай в идеале не должен происходить для GetAccounts, но включен для полноты с паттерном MbResult
        return StatusCode(500, result); 
    }

    /// <summary>
    /// Получить счет по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Счет</returns>
    /// <response code="200">Счет найден</response>
    /// <response code="404">Счет не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MbResult<Account>), 200)]
    [ProducesResponseType(typeof(MbResult<Account>), 404)]
    [ProducesResponseType(typeof(MbResult<Account>), 500)]
    public async Task<ActionResult<MbResult<Account>>> GetAccount(Guid id, CancellationToken cancellationToken)
    {
        GetAccountQuery query = new(id);
        var result = await mediator.Send(query, cancellationToken);
        
        if (result.IsSuccess)
        {
            if (result.Result == null)
            {
                 return NotFound(MbResult<Account>.Failure(new MbError("AccountNotFound", "Счет не найден.")));
            }
            return Ok(result);
        }
        
        // Если результат неудачный, это скорее всего ошибка валидации из предыдущего поведения или общая ошибка
        return StatusCode(500, result);
    }

        /// <summary>
        /// Создать новый счет
        /// </summary>
        /// <param name="request">Данные для создания счета</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Созданный счет</returns>
        /// <remarks>
        /// <h3>Форматы данных:</h3>
        /// <ul>
        /// <li><strong>Decimal:</strong> Используйте точку как разделитель (5.5)</li>
        /// <li><strong>GUID:</strong> Стандартный формат ("11111111-1111-1111-1111-111111111111")</li>
        /// <li><strong>Валюта:</strong> Код ISO 4217 (USD, EUR, RUB)</li>
        /// </ul>
        /// <h3>Пример запроса:</h3>
        /// <pre>
        /// {
        ///   "ownerId": "11111111-1111-1111-1111-111111111111",
        ///   "type": 1,
        ///   "currency": "USD",
        ///   "interestRate": 5.5
        /// }
        /// </pre>
        /// </remarks>
        /// <response code="201">Счет успешно создан</response>
        /// <response code="400">Ошибка валидации</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost]
    [ProducesResponseType(typeof(MbResult<Account>), 201)]
    [ProducesResponseType(typeof(MbResult<Account>), 400)]
    [ProducesResponseType(typeof(MbResult<Account>), 500)]
    public async Task<ActionResult<MbResult<Account>>> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        CreateAccountCommand command = new(request);
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetAccount), new { id = result.Result!.Id }, result);
        }

        if (result.Error!.Code == "ValidationError")
        {
            return BadRequest(result);
        }

        return StatusCode(500, result);
    }

        /// <summary>
        /// Полностью обновить счет
        /// </summary>
        /// <param name="id">Идентификатор счета</param>
        /// <param name="request">Данные для обновления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Обновленный счет</returns>
        /// <remarks>
        /// <h3>Форматы данных:</h3>
        /// <ul>
        /// <li><strong>Decimal:</strong> Используйте точку как разделитель (1000.50, 5.5)</li>
        /// <li><strong>Даты:</strong> Формат ISO 8601 ("2024-01-15T10:30:00")</li>
        /// <li><strong>GUID:</strong> Стандартный формат ("11111111-1111-1111-1111-111111111111")</li>
        /// <li><strong>Валюта:</strong> Код ISO 4217 (USD, EUR, RUB)</li>
        /// </ul>
        /// <h3>Пример запроса:</h3>
        /// <pre>
        /// {
        ///   "ownerId": "11111111-1111-1111-1111-111111111111",
        ///   "type": 1,
        ///   "currency": "USD",
        ///   "balance": 1000.50,
        ///   "interestRate": 5.5,
        ///   "openingDate": "2024-01-15T10:30:00",
        ///   "closingDate": null
        /// }
        /// </pre>
        /// </remarks>
        /// <response code="200">Счет успешно обновлен</response>
        /// <response code="400">Ошибка валидации</response>
        /// <response code="404">Счет не найден</response>
        /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(MbResult<Account>), 200)]
    [ProducesResponseType(typeof(MbResult<Account>), 400)]
    [ProducesResponseType(typeof(MbResult<Account>), 404)]
    [ProducesResponseType(typeof(MbResult<Account>), 500)]
    public async Task<ActionResult<MbResult<Account>>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest request, CancellationToken cancellationToken)
    {
        UpdateAccountCommand command = new(id, request);
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        if (result.Error!.Code == "ValidationError")
        {
            return BadRequest(result);
        }

        if (result.Error!.Code == "AccountNotFound")
        {
            return NotFound(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Частично обновить счет
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    /// <param name="request">Данные для частичного обновления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Обновленный счет</returns>
    /// <response code="200">Счет успешно обновлен</response>
    /// <response code="400">Ошибка валидации</response>
    /// <response code="404">Счет не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(MbResult<Account>), 200)]
    [ProducesResponseType(typeof(MbResult<Account>), 400)]
    [ProducesResponseType(typeof(MbResult<Account>), 404)]
    [ProducesResponseType(typeof(MbResult<Account>), 500)]
    public async Task<ActionResult<MbResult<Account>>> PatchAccount(Guid id, [FromBody] PatchAccountRequest request, CancellationToken cancellationToken)
    {
        PatchAccountCommand command = new(id, request);
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        if (result.Error!.Code == "ValidationError")
        {
            return BadRequest(result);
        }

        if (result.Error!.Code == "AccountNotFound")
        {
            return NotFound(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Удалить счет
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат удаления</returns>
    /// <response code="200">Счет успешно удален</response>
    /// <response code="404">Счет не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(MbResult<bool>), 200)]
    [ProducesResponseType(typeof(MbResult<bool>), 404)]
    [ProducesResponseType(typeof(MbResult<bool>), 500)]
    public async Task<ActionResult<MbResult<bool>>> DeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        DeleteAccountCommand command = new(id);
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        if (result.Error!.Code == "AccountNotFound")
        {
            return NotFound(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Проверить существование счета
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат проверки</returns>
    /// <response code="200">Результат проверки</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id:guid}/exists")]
    [ProducesResponseType(typeof(MbResult<bool>), 200)]
    [ProducesResponseType(typeof(MbResult<bool>), 500)]
    public async Task<ActionResult<MbResult<bool>>> CheckAccountExists(Guid id, CancellationToken cancellationToken)
    {
        CheckAccountExistsQuery query = new(id);
        var result = await mediator.Send(query, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Создать транзакцию для счета
    /// </summary>
    /// <param name="request">Данные транзакции</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданная транзакция</returns>
    /// <response code="201">Транзакция успешно создана</response>
    /// <response code="400">Ошибка валидации или нарушение бизнес-правил (например, недостаточно средств, счет не найден)</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost("transactions")]
    [ProducesResponseType(typeof(MbResult<Transaction>), 201)]
    [ProducesResponseType(typeof(MbResult<Transaction>), 400)]
    [ProducesResponseType(typeof(MbResult<Transaction>), 500)]
    public async Task<ActionResult<MbResult<Transaction>>> CreateTransaction([FromBody] CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        CreateTransactionCommand command = new(request);
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return CreatedAtAction(nameof(GetAccount), new { id = request.AccountId }, result);
        }

        if (result.Error!.Code == "ValidationError" || result.Error!.Code == "InsufficientFunds" || result.Error!.Code == "AccountNotFound")
        {
            return BadRequest(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Выполнить перевод между счетами
    /// </summary>
    /// <param name="request">Данные перевода</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат перевода (DebitTransaction, CreditTransaction)</returns>
    /// <response code="200">Перевод успешно завершен</response>
    /// <response code="400">Ошибка валидации или нарушение бизнес-правил (например, недостаточно средств, счет не найден, несоответствие валют)</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpPost("transfer")]
    [ProducesResponseType(typeof(MbResult<object>), 200)]
    [ProducesResponseType(typeof(MbResult<object>), 400)]
    [ProducesResponseType(typeof(MbResult<object>), 500)]
    public async Task<ActionResult<MbResult<object>>> Transfer([FromBody] TransferRequest request, CancellationToken cancellationToken)
    {
        TransferCommand command = new(request);
        var result = await mediator.Send(command, cancellationToken);
        
        if (result.IsSuccess)
        {
            return Ok(MbResult<object>.Success(new
            {
                Message = "Перевод успешно завершен",
                result.Result!.DebitTransaction,
                result.Result.CreditTransaction
            }));
        }

        if (result.Error!.Code == "ValidationError" || 
            result.Error!.Code == "AccountNotFound" || 
            result.Error!.Code == "InsufficientFunds" || 
            result.Error!.Code == "OwnerMismatch" ||
            result.Error!.Code == "CurrencyMismatch")
        {
            return BadRequest(result);
        }

        return StatusCode(500, result);
    }

    /// <summary>
    /// Получить выписку по счету за указанный период
    /// </summary>
    /// <param name="id">Идентификатор счета</param>
    /// <param name="startDate">Дата начала периода выписки (включительно)</param>
    /// <param name="endDate">Дата окончания периода выписки (включительно)</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Выписка по счету</returns>
    /// <response code="200">Выписка по счету успешно получена</response>
    /// <response code="400">Ошибка валидации (например, неверный диапазон дат)</response>
    /// <response code="404">Счет не найден</response>
    /// <response code="500">Внутренняя ошибка сервера</response>
    [HttpGet("{id:guid}/statement")]
    [ProducesResponseType(typeof(MbResult<AccountStatement>), 200)]
    [ProducesResponseType(typeof(MbResult<AccountStatement>), 400)]
    [ProducesResponseType(typeof(MbResult<AccountStatement>), 404)]
    [ProducesResponseType(typeof(MbResult<AccountStatement>), 500)]
    public async Task<ActionResult<MbResult<AccountStatement>>> GetAccountStatement(
        Guid id, 
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        CancellationToken cancellationToken)
    {
        GetAccountStatementQuery query = new(id, startDate, endDate);
        var result = await mediator.Send(query, cancellationToken);

        if (result.IsSuccess)
        {
            return Ok(result);
        }

        if (result.Error!.Code == "AccountNotFound")
        {
            return NotFound(result);
        }

        if (result.Error!.Code == "ValidationError")
        {
            return BadRequest(result);
        }

        return StatusCode(500, result);
    }
} 