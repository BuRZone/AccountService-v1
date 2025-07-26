using AccountService.API.Commands;
using AccountService.API.Services;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Validators;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by DI container")]
public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator(ICurrencyService currencyService)
    {
        RuleFor(x => x.Request.OwnerId)
            .NotEmpty()
            .WithMessage("ID владельца обязателен")
            .Must(ownerId => ownerId != Guid.Empty)
            .WithMessage("ID владельца не может быть пустым GUID");

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

        RuleFor(x => x.Request.Balance)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Баланс должен быть положительным");

        RuleFor(x => x.Request.InterestRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Request.InterestRate.HasValue)
            .WithMessage("Процентная ставка должна быть положительной");

        RuleFor(x => x.Request.OpeningDate)
            .NotEmpty()
            .WithMessage("Дата открытия обязательна")
            .Must(date => date != DateTime.MinValue && date != DateTime.MaxValue)
            .WithMessage("Дата открытия должна быть валидной датой");

        RuleFor(x => x.Request.ClosingDate)
            .Must(date => !date.HasValue || (date.Value != DateTime.MinValue && date.Value != DateTime.MaxValue))
            .When(x => x.Request.ClosingDate.HasValue)
            .WithMessage("Дата закрытия должна быть валидной датой")
            .GreaterThan(x => x.Request.OpeningDate)
            .When(x => x.Request.ClosingDate.HasValue)
            .WithMessage("Дата закрытия должна быть после даты открытия");
    }
} 