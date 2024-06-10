using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public interface IAccountRepository
    {
        IEnumerable<Account> FindAllAccounts(); 
        Account FindAccountById(long accountId);
       void Save(Account account);
        IEnumerable<Account> GetAccountsByClient(long clientId);
    }
}
