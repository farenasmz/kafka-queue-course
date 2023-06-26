using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handler;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.WithDrawnFund
{
    public class WithDrawFundsCommandHandler : IRequestHandler<WithdrawFundsCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> EventSourcingHandler;

        public WithDrawFundsCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            EventSourcingHandler = eventSourcingHandler;
        }

        public async Task<bool> Handle(WithdrawFundsCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await EventSourcingHandler.GetById(request.Id);
            aggregate.WithdrawFunds(request.Amount);
            await EventSourcingHandler.Save(aggregate);
            return true;
        }
    }
}
