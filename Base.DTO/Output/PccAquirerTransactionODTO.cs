namespace Base.DTO.Output
{
    public class PccAquirerTransactionODTO
    {
        public int AquirerTransctionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }
        public string IssuerAccountNumber { get; set; }
        public int IssuerTransactionId { get; set; }
        public DateTime IssuerTimestamp { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsSuccess { get; set; }

        public PccAquirerTransactionODTO(string accountNumber, string currencyCode)
        {
            IssuerAccountNumber = accountNumber;
            CurrencyCode = currencyCode;
        }

        public PccAquirerTransactionODTO()
        {
            IssuerAccountNumber = string.Empty;
            CurrencyCode = string.Empty;
        }
    }
}
