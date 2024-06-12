using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly HomeBankingContext _loanRepository;
        public LoanRepository(HomeBankingContext loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public IEnumerable<Loan> GetAll() 
        {
            return _loanRepository.Loans.ToList();
        }

        public Loan FindById(long Id) 
        {
            return _loanRepository.Loans.FirstOrDefault(loan => loan.Id == Id);
        }
    }
}
