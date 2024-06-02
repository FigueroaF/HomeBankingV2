namespace HomeBankingV1.DTOS
{
    public class ClientLoanDTO
    {
        public long id {  get; set; }
        public long LoanId { get; set; }
        public string Name {  get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }
    }
}
