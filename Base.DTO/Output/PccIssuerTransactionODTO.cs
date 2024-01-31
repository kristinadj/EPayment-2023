namespace Base.DTO.Output
{
    public class PccIssuerTransactionODTO
    {
        public int IssuerTransctionId { get; set; }
        public DateTime IssuerTimestamp { get; set; }
        public int AcquirerTransactionId { get; set; }
        public DateTime AcquirerTimestamp { get; set; }
        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public bool IsSuccess { get; set; }

        public PccIssuerTransactionODTO(string currencyCode)
        {
            CurrencyCode = currencyCode;
        }

        public PccIssuerTransactionODTO()
        {
            CurrencyCode = string.Empty;
        }
    }
}
