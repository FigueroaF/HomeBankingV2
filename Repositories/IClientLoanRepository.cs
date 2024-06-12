using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public interface IClientLoanRepository
    {
        void Save(ClientLoan clientLoan);
    }
}
