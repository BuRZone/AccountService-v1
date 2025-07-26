using AccountService.API.Commands;
using AccountService.API.DTOs;
using AccountService.API.Models;
using AccountService.API.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AccountsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Получить список всех счетов
    /// </summary>
    /// <param name="ownerId">ID владельца для фильтрации (опционально)</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список счетов</returns>
    /// <response code="200">Список счетов успешно получен</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<Account>), 200)]
    public async Task<ActionResult<List<Account>>> GetAccounts([FromQuery] Guid? ownerId, CancellationToken cancellationToken)
    {
        GetAccountsQuery query = new(ownerId);
        var accounts = await mediator.Send(query, cancellationToken);
        return Ok(accounts);
    }

    /// <summary>
    /// Получить счет по ID
    /// </summary>
    /// <param name="id">ID счета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Счет</returns>
    /// <response code="200">Счет найден</response>
    /// <response code="404">Счет не найден</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Account), 200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Account>> GetAccount(Guid id, CancellationToken cancellationToken)
    {
        GetAccountQuery query = new(id);
        var account = await mediator.Send(query, cancellationToken);
        
        if (account == null)
            return NotFound();
            
        return Ok(account);
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
        /// <li><strong>Валюта:</strong> ISO 4217 код (USD, EUR, RUB)</li>
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
    [HttpPost]
    [ProducesResponseType(typeof(Account), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Account>> CreateAccount([FromBody] CreateAccountRequest request, CancellationToken cancellationToken)
    {
        CreateAccountCommand command = new(request);
        var account = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAccount), new { id = account.Id }, account);
    }

        /// <summary>
        /// Обновить счет целиком
        /// </summary>
        /// <param name="id">ID счета</param>
        /// <param name="request">Данные для обновления</param>
        /// <param name="cancellationToken">Токен отмены операции</param>
        /// <returns>Обновленный счет</returns>
        /// <remarks>
        /// <h3>Форматы данных:</h3>
        /// <ul>
        /// <li><strong>Decimal:</strong> Используйте точку как разделитель (1000.50, 5.5)</li>
        /// <li><strong>Даты:</strong> ISO 8601 формат ("2024-01-15T10:30:00")</li>
        /// <li><strong>GUID:</strong> Стандартный формат ("11111111-1111-1111-1111-111111111111")</li>
        /// <li><strong>Валюта:</strong> ISO 4217 код (USD, EUR, RUB)</li>
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
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(Account), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Account>> UpdateAccount(Guid id, [FromBody] UpdateAccountRequest request, CancellationToken cancellationToken)
    {
        UpdateAccountCommand command = new(id, request);
        var account = await mediator.Send(command, cancellationToken);
        return Ok(account);
    }

    /// <summary>
    /// Частично обновить счет
    /// </summary>
    /// <param name="id">ID счета</param>
    /// <param name="request">Данные для частичного обновления</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Обновленный счет</returns>
    /// <response code="200">Счет успешно обновлен</response>
    /// <response code="400">Ошибка валидации</response>
    /// <response code="404">Счет не найден</response>
    [HttpPatch("{id:guid}")]
    [ProducesResponseType(typeof(Account), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<Account>> PatchAccount(Guid id, [FromBody] PatchAccountRequest request, CancellationToken cancellationToken)
    {
        PatchAccountCommand command = new(id, request);
        var account = await mediator.Send(command, cancellationToken);
        return Ok(account);
    }

    /// <summary>
    /// Удалить счет
    /// </summary>
    /// <param name="id">ID счета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат удаления</returns>
    /// <response code="200">Счет успешно удален</response>
    /// <response code="404">Счет не найден</response>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        DeleteAccountCommand command = new(id);
        var deleted = await mediator.Send(command, cancellationToken);
        
        if (!deleted)
            return NotFound();
            
        return Ok();
    }

    /// <summary>
    /// Проверить существование счета
    /// </summary>
    /// <param name="id">ID счета</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат проверки</returns>
    /// <response code="200">Результат проверки</response>
    [HttpGet("{id:guid}/exists")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<ActionResult<bool>> CheckAccountExists(Guid id, CancellationToken cancellationToken)
    {
        CheckAccountExistsQuery query = new(id);
        var exists = await mediator.Send(query, cancellationToken);
        return Ok(exists);
    }

    /// <summary>
    /// Создать транзакцию по счету
    /// </summary>
    /// <param name="request">Данные транзакции</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданная транзакция</returns>
    /// <response code="201">Транзакция успешно создана</response>
    /// <response code="400">Ошибка валидации</response>
    [HttpPost("transactions")]
    [ProducesResponseType(typeof(Transaction), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<Transaction>> CreateTransaction([FromBody] CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        CreateTransactionCommand command = new(request);
        var transaction = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAccount), new { id = request.AccountId }, transaction);
    }

    /// <summary>
    /// Выполнить перевод между счетами
    /// </summary>
    /// <param name="request">Данные перевода</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат перевода</returns>
    /// <response code="200">Перевод выполнен успешно</response>
    /// <response code="400">Ошибка валидации</response>
    [HttpPost("transfer")]
    [ProducesResponseType(typeof(object), 200)]
    [ProducesResponseType(400)]
    public async Task<ActionResult> Transfer([FromBody] TransferRequest request, CancellationToken cancellationToken)
    {
        TransferCommand command = new(request);
        var result = await mediator.Send(command, cancellationToken);
        
        return Ok(new
        {
            Message = "Transfer completed successfully",
            result.DebitTransaction,
            result.CreditTransaction
        });
    }
} 