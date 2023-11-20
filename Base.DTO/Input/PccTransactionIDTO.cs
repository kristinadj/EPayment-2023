namespace Base.DTO.Input
{
    public class PccTransactionIDTO
    {
        public int AquirerBankId { get; set; }
        public int AquirerTransctionId { get; set; }
        public DateTime AquirerTimestamp { get; set; }

        public double Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string Description { get; set; }

        public PayTransactionIDTO? PayTransaction { get; set; }

        public PccTransactionIDTO(string currecncyCode, string description)
        {
            CurrencyCode = currecncyCode;
            Description = description;
        }

        public PccTransactionIDTO()
        {
            CurrencyCode = string.Empty;
            Description = string.Empty;
        }
    }
}
