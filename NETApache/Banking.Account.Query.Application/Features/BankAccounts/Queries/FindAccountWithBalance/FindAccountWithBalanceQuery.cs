using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAccountWithBalance
{
    public class FindAllAccountsQuery : IRequest<IEnumerable<BankAccount>>
    {
        public decimal Balance { get; set; }
        public string EqualityType { get; set; } = string.Empty;
    }
}
