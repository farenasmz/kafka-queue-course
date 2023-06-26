using Banking.Account.Query.Application.Contracts.Persistence;
using Banking.Account.Query.Domain;
using Banking.Account.Query.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Banking.Account.Query.Infrastructure.Repository
{
    public class BankAccountRepository : RepositoryBase<BankAccount>, IBankAccountRepository
    {
        protected BankAccountRepository(MySqlDbContext context) : base(context)
        {

        }

        public async Task DeleteByIdentifier(string identifier)
        {
            var bankAccount = await _context.BankAccounts.Where(x => x.Identifier == identifier).FirstOrDefaultAsync() ?? throw new ArgumentException("Not found");
            _context.BankAccounts.Remove(bankAccount);
        }

        public async Task DepositBankAccountByIdentifier(BankAccount bankAccount)
        {
            var account = await _context.BankAccounts.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync() ?? throw new ArgumentException("Not found");
            account.Balance += bankAccount.Balance;
            await UpdateAsync(account);
        }

        public async Task<IEnumerable<BankAccount>> FindByAccountHolder(string accountHolder)
        {
            return await _context.BankAccounts.Where(x => x.AccountHolder == accountHolder).ToListAsync() ?? throw new ArgumentException("Not found");
        }

        public async Task<BankAccount> FindByAccountIdentifier(string identifier)
        {
            return await _context.BankAccounts.Where(x => x.Identifier == identifier).FirstOrDefaultAsync() ?? throw new ArgumentException("Not found");
        }

        public async Task<IEnumerable<BankAccount>> FindByBalanceGreaterThan(decimal balance)
        {
            return await _context.BankAccounts.Where(x => x.Balance > balance).ToListAsync() ?? throw new ArgumentException("Not found");
        }

        public async Task<IEnumerable<BankAccount>> FindByBalanceLessThan(decimal balance)
        {
            return await _context.BankAccounts.Where(x => x.Balance < balance).ToListAsync() ?? throw new ArgumentException("Not found");
        }

        public async Task WithdrawnBankAccountByIdentifier(BankAccount bankAccount)
        {
            var account = await _context.BankAccounts.Where(x => x.Identifier == bankAccount.Identifier).FirstOrDefaultAsync() ?? throw new ArgumentException("Not found");

            if (bankAccount.Balance < account.Balance)
            {
                throw new ArgumentException("Not enough balance");
            }

            account.Balance -= bankAccount.Balance;
            await UpdateAsync(account);
        }
    }
}
