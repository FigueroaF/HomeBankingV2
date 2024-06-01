using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public interface IClientRepository
    {
        IEnumerable<Client> GetAllClients();
        void Save(Client client);
        Client FindById(long id);
    }
}
