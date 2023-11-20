namespace Base.DTO.Output
{
    public class PccTransactionODTO
    {
        public int AquirerTransctionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }
        public string IssuerAccountNumber { get; set; }
        public int? IssuerTransactionId { get; set; }
        public DateTime IssuerTimestamp { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsSuccess { get; set; }

        public PccTransactionODTO(string accountNumber, string currencyCode)
        {
            IssuerAccountNumber = accountNumber;
            CurrencyCode = currencyCode;
        }

        public PccTransactionODTO()
        {
            IssuerAccountNumber = string.Empty;
            CurrencyCode = string.Empty;
        }
    }
}
