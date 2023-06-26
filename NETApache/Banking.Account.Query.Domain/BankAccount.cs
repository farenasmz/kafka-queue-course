using Banking.Account.Query.Domain.Common;

namespace Banking.Account.Query.Domain
{
    public class BankAccount : BaseDomainModel
    {
        public string Identifier { get; set; }

        public string AccountHolder { get; set; }

        public DateTime CreationDate { get; set; }

        public string AccountType { get; set; }

        public decimal Balance { get; set; }
    }
}
