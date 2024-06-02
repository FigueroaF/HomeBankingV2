using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public interface IAccountRepository
    {
        List<Account> GetAllAccounts(); 
        Account GetAccountById(long accountId);
        Account GetAccountById();
    }
}
