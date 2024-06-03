using HomeBankingV1.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingV1.Repositories.Implementations
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindById(long id)
        {
            return FindByCondition(c => c.Id == id)
                 .Include(c => c.Accounts)
                 .Include(c => c.ClientLoans)
                 .ThenInclude(cl => cl.Loan)
                 .Include(client => client.Cards)
                 .FirstOrDefault();
        }
        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper())
            .Include(client => client.Accounts)
            .Include(client => client.ClientLoans)
                .ThenInclude(cl => cl.Loan)
            .Include(client => client.Cards)
            .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
           return FindAll()
                .Include(c => c.Accounts)
                .Include(c => c.ClientLoans)
                .ThenInclude(cl => cl.Loan)
                .Include(client => client.Cards)
                .ToList();
        }

        public void Save(Client client)
        {
            Create(client);
            SaveChanges();
        }
    }
}
