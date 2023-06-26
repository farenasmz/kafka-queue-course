using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountWithBalance
{
    public class FindAllAccountsQueryHandler : IRequestHandler<FindAllAccountsQuery, IEnumerable<BankAccount>>
    {
        private readonly IBankAccountRepository bankAccountRepository;

        public FindAllAccountsQueryHandler(IBankAccountRepository bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<IEnumerable<BankAccount>> Handle(FindAllAccountsQuery request, CancellationToken cancellationToken)
        {
            if (request.EqualityType == "GREATER_THAN")
            {
                return await bankAccountRepository.FindByBalanceGreaterThan(request.Balance);
            }

            return await bankAccountRepository.FindByBalanceLessThan(request.Balance);
        }
    }
}
