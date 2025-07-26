using AccountService.API.Commands;
using AccountService.API.Services;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Validators;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by DI container")]
public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator(ICurrencyService currencyService)
    {
        RuleFor(x => x.Request.AccountId)
            .NotEmpty()
            .WithMessage("ID счета обязателен")
            .Must(accountId => accountId != Guid.Empty)
            .WithMessage("ID счета не может быть пустым GUID");

        RuleFor(x => x.Request.Amount)
            .GreaterThan(0)
            .WithMessage("Сумма должна быть больше нуля");

        RuleFor(x => x.Request.Currency)
            .NotEmpty()
            .WithMessage("Валюта обязательна")
            .Must(currency => !string.IsNullOrWhiteSpace(currency))
            .WithMessage("Валюта не может быть пустой строкой или пробелами")
            .Must(currency => currency.Length == 3)
            .WithMessage("Валюта должна содержать ровно 3 символа (формат ISO 4217)")
            .MustAsync(async (currency, cancellation) => await currencyService.IsSupportedCurrencyAsync(currency, cancellation))
            .WithMessage("Неподдерживаемая валюта");

        RuleFor(x => x.Request.Type)
            .IsInEnum()
            .WithMessage("Неверный тип транзакции");

        RuleFor(x => x.Request.Description)
            .NotEmpty()
            .WithMessage("Описание обязательно")
            .Must(description => !string.IsNullOrWhiteSpace(description))
            .WithMessage("Описание не может быть пустой строкой или пробелами")
            .MaximumLength(500)
            .WithMessage("Описание не должно превышать 500 символов");

        RuleFor(x => x.Request.CounterpartyAccountId)
            .Must(id => !id.HasValue || id.Value != Guid.Empty)
            .WithMessage("ID счета контрагента не может быть пустым GUID")
            .NotEqual(x => x.Request.AccountId)
            .When(x => x.Request.CounterpartyAccountId.HasValue)
            .WithMessage("ID счета контрагента не может совпадать с ID счета");
    }
} 