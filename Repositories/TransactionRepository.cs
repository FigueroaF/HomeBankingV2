using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext) 
        { }
        public void Save(Transaction transaction) 
        {
            Create (transaction);
            SaveChanges();
        }
        public void SaveTransaction(HomeBankingV1.Models.Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }

    }
}
