using HomeBankingV1.Models;

namespace HomeBankingV1.DTOS
{
    public class LoanDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }


        public LoanDTO(Loan loan)
        {
            Id = loan.Id;
            Name = loan.Name;
            MaxAmount = loan.MaxAmount;
            Payments = loan.Payments;
        }

    }

}
