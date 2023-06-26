using Banking.Account.Query.Domain;
using MediatR;

namespace Banking.Account.Query.Application.Features.BankAccounts.Queries.FindAllAccounts
{
    public class FindAllAccountsQuery : IRequest<IEnumerable<BankAccount>>
    {
        public float Balance { get; set; }
        public string EqualityType { get; set; } = string.Empty;
    }
}
