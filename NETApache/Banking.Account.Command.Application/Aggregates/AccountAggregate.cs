using Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount;
using Banking.Cqrs.Core.Domain;
using Banking.Cqrs.Core.Event;

namespace Banking.Account.Command.Application.Aggregates
{
    public class AccountAggregate : AggregateRoot
    {
        public string AccountNumber { get; set; }
        public string CustomerId { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }

        public AccountAggregate()
        {
            
        }

        public AccountAggregate(OpenAccountCommand command)
        {
            var accountOpenedEvent = new AccountOpenedEvent(command.Id, command.AccountHolder, command.AccountType, DateTime.Now, command.OpeningBalance);
            RaiseEvent(accountOpenedEvent);
        }

        public void Apply(AccountOpenedEvent @event)
        {
            Id = @event.Id;
            IsActive = true;
            Balance = @event.OpeningBalance;
        }

        public void DepositFunds(decimal amount)
        {
            if (!IsActive)
            {
                throw new ArgumentException("Inactive account");
            }

            if (amount < 0)
            {
                throw new ArgumentException("Not money bro!");
            }

            var fundsDepositEvent = new FundsDepositedEvent(Id, amount);
            RaiseEvent(fundsDepositEvent);
        }

        public void Apply(FundsDepositedEvent @event)
        {
            Id = @event.Id;
            IsActive = true;
            Balance += @event.Amount;
        }

        public void WithdrawFunds(decimal amount)
        {
            if (!IsActive)
            {
                throw new ArgumentException("Inactive account");
            }

            if (amount < 0)
            {
                throw new ArgumentException("Not money bro!");
            }

            var fundsWithdrawEvent = new FundsWithDrawnEvent(Id)
            {
                Id = Id,
                Amount = amount,
            };

            RaiseEvent(fundsWithdrawEvent);
        }

        public void Apply(FundsWithDrawnEvent @event)
        {
            Id = @event.Id;
            Balance -= @event.Amount;
        }

        public void CloseAccount()
        {
            if (!IsActive)
            {
                throw new ArgumentException("Inactive account");
            }

            var accountClosedEvent = new AccountClosedEvent(Id);
            RaiseEvent(accountClosedEvent);
        }

        public void Apply(AccountClosedEvent @event)
        {
            Id = @event.Id;
            IsActive = false;
        }
    }
}
