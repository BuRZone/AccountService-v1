using AccountService.API.Commands;
using AccountService.API.Services;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Validators;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by DI container")]
public class TransferCommandValidator : AbstractValidator<TransferCommand>
{
    public TransferCommandValidator(ICurrencyService currencyService)
    {
        RuleFor(x => x.Request.FromAccountId)
            .NotEmpty()
            .WithMessage("ID счета отправителя обязателен")
            .Must(accountId => accountId != Guid.Empty)
            .WithMessage("ID счета отправителя не может быть пустым GUID");

        RuleFor(x => x.Request.ToAccountId)
            .NotEmpty()
            .WithMessage("ID счета получателя обязателен")
            .Must(accountId => accountId != Guid.Empty)
            .WithMessage("ID счета получателя не может быть пустым GUID");

        RuleFor(x => x.Request.FromAccountId)
            .NotEqual(x => x.Request.ToAccountId)
            .WithMessage("Счета отправителя и получателя должны быть разными");

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

        RuleFor(x => x.Request.Description)
            .NotEmpty()
            .WithMessage("Описание обязательно")
            .Must(description => !string.IsNullOrWhiteSpace(description))
            .WithMessage("Описание не может быть пустой строкой или пробелами")
            .MaximumLength(500)
            .WithMessage("Описание не должно превышать 500 символов");
    }
} 