namespace WebShop.DTO.Output
{
    public class InvoiceODTO
    {
        public int InvoiceId { get; set; }
        public int MerchantId { get; set; }
        public double TotalPrice { get; set; }
        public int CurrencyId { get; set; }
        public int TransactionId { get; set; }

        public OrderODTO? Order { get; set; }
        public CurrencyODTO? Currency { get; set; }
        public TransactionODTO? Transaction { get; set; }
    }
}
