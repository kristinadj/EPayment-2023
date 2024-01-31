using Bank.DTO.Input;

namespace Base.DTO.Input
{
    public class PccAquirerTransactionIDTO
    {
        public int AquirerBankId { get; set; }
        public int AquirerTransctionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }

        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }

        public int ExternalInvoiceId {  get; set; }
        public string TransactionSuccessUrl { get; set; } = string.Empty;
        public string TransactionFailureUrl { get; set; } = string.Empty;
        public string TransactionErrorUrl { get; set; } = string.Empty;

        public PayTransactionIDTO? PayTransaction { get; set; }

        public PccAquirerTransactionIDTO(string currecncyCode, string description) : base()
        {
            CurrencyCode = currecncyCode;
            Description = description;
        }

        public PccAquirerTransactionIDTO()
        {
            CurrencyCode = string.Empty;
            Description = string.Empty;
        }
    }
}
