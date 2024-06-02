using HomeBankingV1.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingV1.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private HomeBankingContext _DbContext;

       public AccountRepository(HomeBankingContext dbContext)
        {
            _DbContext = dbContext;
        }
        public List<Account> GetAllAccounts()
        {
            return _DbContext.Accounts.ToList();
        }
        public Account GetAccountById(long accountid)
        {
            return _DbContext.Accounts.FirstOrDefault(a => a.Id == accountid);

        }

        public Account GetAccountById()
        {
            throw new NotImplementedException();
        }
    }
}
