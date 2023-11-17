namespace PSP.WebApi.DTO.Output
{
    public class InvoiceODTO
    {
        public int ExtrenalInvoiceId { get; set; }
        public int MerchantId { get; set; }
        public int IssuedToUserId { get; set; }
        public double TotalPrice { get; set; }
        public string CurrencyCode { get; set; }
        public string? RedirectUrl { get; set; }
    }
}
