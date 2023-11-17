namespace PSP.WebApi.DTO.Input
{
    public class InvoiceIDTO
    {
        public int ExtrenalInvoiceId { get; set; }
        public int MerchantId { get; set; }
        public int IssuedToUserId { get; set; }
        public double TotalPrice { get; set; }
        public string CurrencyCode { get; set; }

        public InvoiceIDTO(string currencyCode)
        {
            CurrencyCode = currencyCode;
        }
    }
}
