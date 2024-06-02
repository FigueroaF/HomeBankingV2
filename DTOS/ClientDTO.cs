using HomeBankingV1.Models;
using System.Linq.Expressions;

namespace HomeBankingV1.DTOS
{
    public class ClientDTO
    {
        public long Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public ICollection<ClientAccountDTO> Accounts { get; set; }
        public ICollection<ClientLoanDTO> Loans { get; set; }
        public ClientDTO(Client client)
        {
            Id = client.Id;
            FirstName = client.FirstName;
            LastName = client.LastName;
            Email = client.Email;
            Accounts = client.Accounts.Select(a => new ClientAccountDTO(a)).ToList();{
            Loans = client.ClientLoans.Select(cl => new ClientLoanDTO
            {
                id = cl.Id,
                LoanId = cl.LoanId,
                Name = cl.Loan.Name,
                Amount = cl.Amount,
                Payments = int.Parse(cl.Payments)}).ToList();
            }
        }
    }
}

