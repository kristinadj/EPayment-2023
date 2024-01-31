namespace Base.DTO.Input
{
    public class PccIssuerTransactionIDTO
    {
        public int IssuerBankId { get; set; }
        public int IssuerTransctionId { get; set; }
        public DateTime IssuerTimestamp { get; set; }

        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }

        public string AcquirerAccountNumber { get; set; }
        public bool UseHyphens { get; set; }
        public int ExternalInvoiceId { get; set; }
        public string TransactionSuccessUrl { get; set; } = string.Empty;
        public string TransactionFailureUrl { get; set; } = string.Empty;
        public string TransactionErrorUrl { get; set; } = string.Empty;

        public PccIssuerTransactionIDTO(string currecncyCode, string description, string acquirerAccountNumber)
        {
            CurrencyCode = currecncyCode;
            Description = description;
            AcquirerAccountNumber = acquirerAccountNumber;
        }

        public PccIssuerTransactionIDTO()
        {
            CurrencyCode = string.Empty;
            Description = string.Empty;
            AcquirerAccountNumber = string.Empty;
        }
    }
}
