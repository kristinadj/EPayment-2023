using Base.DTO.Enums;

namespace WebShop.DTO.Output
{
    public class InvoiceODTO
    {
        public int InvoiceId { get; set; }
        public InvoiceType InvoiceType { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; } = string.Empty;
        public UserODTO? User { get; set; } 
        public MerchantODTO? Merchant { get; set; }
        public CurrencyODTO? Currency { get; set; }
        public TransactionODTO? Transaction { get; set; }

        public MerchantOrderODTO? MerchantOrder { get; set; }
        public UserSubscriptionPlanODTO? UserSubscriptionPlan { get; set; }
    }
}
