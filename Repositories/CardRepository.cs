using HomeBankingV1.Models;

namespace HomeBankingV1.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly HomeBankingContext _cardRepository;

        public CardRepository(HomeBankingContext cardRepository)
        {
            _cardRepository = cardRepository;
        }

        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return _cardRepository.Cards.Where(c => c.ClientId == clientId).ToList();
        }

        public void Save(Card card)
        {
            _cardRepository.Cards.Add(card);
            _cardRepository.SaveChanges();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return _cardRepository.Cards.ToList();
        }

        public Card FindById(long id)
        {
            return _cardRepository.Cards.Find(id);
        }

        public bool IsCardNumberUnique(string cardNumber)
        {
            return !_cardRepository.Cards.Any(c => c.Number == cardNumber);
        }

        public bool CardNumberExists(string cardNumber)
        {
            return _cardRepository.Cards.Any(c => c.Number == cardNumber);
        }
    }
}
