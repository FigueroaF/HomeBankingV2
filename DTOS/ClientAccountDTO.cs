using HomeBankingV1.Models;

namespace HomeBankingV1.DTOS
{
    public class ClientAccountDTO
    {
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime CreationDate { get; set; }
        public double Balance { get; set; }
        public ClientAccountDTO(Account account)
        {
            Id = account.Id;
            Number = account.Number;
            CreationDate = account.CreationDate;
            Balance = account.Balance;
        }
    }
}
