namespace Base.DTO.Shared
{
    public class PspInvoiceIDTO
    {
        public int ExternalInvoiceId { get; set; }
        public int MerchantId { get; set; }
        public string IssuedToUserId { get; set; }
        public double TotalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string? RedirectUrl { get; set; } = string.Empty;

        public PspInvoiceIDTO(string issuedToUser, string currencyCode)
        {
            IssuedToUserId = issuedToUser;
            CurrencyCode = currencyCode;
        }

        public PspInvoiceIDTO()
        {
            IssuedToUserId = string.Empty;
            CurrencyCode = string.Empty;
        }
    }
}
