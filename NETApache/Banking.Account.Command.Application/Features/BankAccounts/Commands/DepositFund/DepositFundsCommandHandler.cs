using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handler;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.DepositFund
{
    public class DepositFundsCommandHandler : IRequestHandler<DepositFundsCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> EventSourcingHandler;

        public DepositFundsCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            EventSourcingHandler = eventSourcingHandler;
        }

        public async Task<bool> Handle(DepositFundsCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await EventSourcingHandler.GetById(request.Id);
            aggregate.DepositFunds(request.Amount);
            await EventSourcingHandler.Save(aggregate);
            return true;
        }
    }
}
