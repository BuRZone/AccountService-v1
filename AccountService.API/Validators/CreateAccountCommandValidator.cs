using AccountService.API.Commands;
using AccountService.API.Models;
using AccountService.API.Services;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Validators;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by DI container")]
public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(IClientVerificationService clientVerificationService, ICurrencyService currencyService)
    {
        RuleFor(x => x.Request.OwnerId)
            .NotEmpty()
            .WithMessage("ID владельца обязателен")
            .Must(ownerId => ownerId != Guid.Empty)
            .WithMessage("ID владельца не может быть пустым GUID")
            .MustAsync(async (ownerId, cancellation) => await clientVerificationService.ClientExistsAsync(ownerId, cancellation))
            .WithMessage("Клиент не существует");

        RuleFor(x => x.Request.Type)
            .IsInEnum()
            .WithMessage("Неверный тип счета");

        RuleFor(x => x.Request.Currency)
            .NotEmpty()
            .WithMessage("Валюта обязательна")
            .Must(currency => !string.IsNullOrWhiteSpace(currency))
            .WithMessage("Валюта не может быть пустой строкой или пробелами")
            .Must(currency => currency.Length == 3)
            .WithMessage("Валюта должна содержать ровно 3 символа (формат ISO 4217)")
            .MustAsync(async (currency, cancellation) => await currencyService.IsSupportedCurrencyAsync(currency, cancellation))
            .WithMessage("Неподдерживаемая валюта");

        RuleFor(x => x.Request.InterestRate)
            .NotNull()
            .When(x => x.Request.Type is AccountType.Deposit or AccountType.Credit)
            .WithMessage("Процентная ставка обязательна для депозитных и кредитных счетов")
            .GreaterThanOrEqualTo(0)
            .When(x => x.Request.InterestRate.HasValue)
            .WithMessage("Процентная ставка должна быть положительной");
    }
} 