using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.WithDrawnFund
{
    public class WithdrawFundsCommand : IRequest<bool>
    {
        public string Id { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
