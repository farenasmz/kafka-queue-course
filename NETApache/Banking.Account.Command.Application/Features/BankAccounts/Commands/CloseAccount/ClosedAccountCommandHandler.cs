using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handler;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.CloseAccount
{
    public class ClosedAccountCommandHandler : IRequestHandler<CloseAccountCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> EventSourcingHandler;

        public ClosedAccountCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            EventSourcingHandler = eventSourcingHandler;
        }

        public async Task<bool> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var aggregate = await EventSourcingHandler.GetById(request.Id);

            if (aggregate != null)
            {
                await EventSourcingHandler.Save(aggregate);
                return true;
            }

            return false;
        }
    }
}
