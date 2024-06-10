using System.Transactions;

namespace HomeBankingV1.Repositories
{
    public interface ITransactionRepository
    {
        void Save(HomeBankingV1.Models.Transaction transaction);
    }
}
