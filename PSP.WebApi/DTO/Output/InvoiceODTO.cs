namespace PSP.WebApi.DTO.Output
{
    public class InvoiceODTO
    {
        public int ExternalInvoiceId { get; set; }
        public int MerchantId { get; set; }
        public string IssuedToUserId { get; set; }
        public double TotalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
