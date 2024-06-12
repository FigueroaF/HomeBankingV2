using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public class ClientLoanRepository : IClientLoanRepository
    {
        private readonly HomeBankingContext _clientloanRepository;
        public ClientLoanRepository (HomeBankingContext clientloanRepository) 
        {
            _clientloanRepository = clientloanRepository;
        }

        public void Save(ClientLoan clientLoan) 
        {
            _clientloanRepository.ClientLoans.Add(clientLoan);
            _clientloanRepository.SaveChanges();
        }
    }
}
