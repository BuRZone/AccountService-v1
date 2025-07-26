using System.Diagnostics.CodeAnalysis;

namespace AccountService.API.Models;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
[SuppressMessage("ReSharper", "AutoPropertyCanBeMadeGetOnly.Global")]
[SuppressMessage("ReSharper", "PropertyCanBeMadeInitOnly.Global")]
public class Account
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public decimal? InterestRate { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime? ClosingDate { get; set; }
    public List<Transaction> Transactions { get; set; } = [];
}

[SuppressMessage("ReSharper", "UnusedMember.Global")]
public enum AccountType
{
    Checking,
    Deposit,
    Credit
}