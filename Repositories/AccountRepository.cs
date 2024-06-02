using HomeBankingV1.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingV1.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        //private HomeBankingContext _DbContext;

        public AccountRepository(HomeBankingContext dbContext) : base(dbContext)
        {
           //dbContext = dbContext;
        }
        public IEnumerable<Account> FindAllAccounts()
        {
            return FindAll()
            .Include(a => a.Transaction)
            .ToList();
        }
        public Account FindAccountById(long accountid)
        {
            return FindByCondition(a => a.Id == accountid)
            .Include(a => a.Transaction)
            .FirstOrDefault();


        }

        public void Save(Account account)
        {
            throw new NotImplementedException();
        }
    }
}
