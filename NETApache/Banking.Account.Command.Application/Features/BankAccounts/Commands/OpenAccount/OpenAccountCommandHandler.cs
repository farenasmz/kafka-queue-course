using Banking.Account.Command.Application.Aggregates;
using Banking.Cqrs.Core.Handler;
using MediatR;

namespace Banking.Account.Command.Application.Features.BankAccounts.Commands.OpenAccount
{
    public class OpenAccountCommandHandler : IRequestHandler<OpenAccountCommand, bool>
    {
        private readonly IEventSourcingHandler<AccountAggregate> EventSourcingHandler;

        public OpenAccountCommandHandler(IEventSourcingHandler<AccountAggregate> eventSourcingHandler)
        {
            EventSourcingHandler = eventSourcingHandler;
        }

        public async Task<bool> Handle(OpenAccountCommand request, CancellationToken cancellationToken)
        {
            var aggregate = new AccountAggregate(request);
            await EventSourcingHandler.Save(aggregate);
            return true;
        }
    }
}
