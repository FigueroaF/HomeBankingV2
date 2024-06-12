using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public interface ILoanRepository
    {
        IEnumerable<Loan> GetAll();
        Loan FindById(long id);
    }
}
