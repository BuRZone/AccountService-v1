using AccountService.API.DTOs;
using AccountService.API.Services;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Validators;

[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Used by DI container")]
public class PatchAccountRequestValidator : AbstractValidator<PatchAccountRequest>
{
    public PatchAccountRequestValidator(IClientVerificationService clientVerificationService, ICurrencyService currencyService)
    {
        RuleFor(x => x.OwnerId)
            .Must(ownerId => !ownerId.HasValue || ownerId.Value != Guid.Empty)
            .When(x => x.OwnerId.HasValue)
            .WithMessage("ID владельца не может быть пустым GUID")
            .MustAsync(async (ownerId, cancellation) => 
                !ownerId.HasValue || await clientVerificationService.ClientExistsAsync(ownerId.Value, cancellation))
            .When(x => x.OwnerId.HasValue)
            .WithMessage("Клиент не существует");

        RuleFor(x => x.Type)
            .IsInEnum()
            .When(x => x.Type.HasValue)
            .WithMessage("Неверный тип счета");

        RuleFor(x => x.Currency)
            .MustAsync(async (currency, cancellation) => 
                string.IsNullOrEmpty(currency) || await currencyService.IsSupportedCurrencyAsync(currency, cancellation))
            .When(x => !string.IsNullOrEmpty(x.Currency))
            .WithMessage("Неподдерживаемая валюта");

        RuleFor(x => x.Balance)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Balance.HasValue)
            .WithMessage("Баланс должен быть положительным");

        RuleFor(x => x.InterestRate)
            .GreaterThanOrEqualTo(0)
            .When(x => x.InterestRate.HasValue)
            .WithMessage("Процентная ставка должна быть положительной");

        RuleFor(x => x.ClosingDate)
            .GreaterThan(x => x.OpeningDate)
            .When(x => x.ClosingDate.HasValue && x.OpeningDate.HasValue)
            .WithMessage("Дата закрытия должна быть после даты открытия");
    }
} 