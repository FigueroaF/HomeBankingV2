using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        Card FindById(long id);
        void Save(Card card);
        IEnumerable<Card> GetCardsByClient(long clientId); // Método para obtener todas las tarjetas de un cliente específico
        bool CardNumberExists(string cardNumber);
        bool IsCardNumberUnique(string cardNumber);
    }
}
