using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAccounts
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
            return await bankAccountRepository.GetAllAsync();
        }
    }
}
