namespace WebShop.DTO.Output
{
    public class PaymentMethodMerchantODTO
    {
        public int PaymentMethodMerchantId { get; set; }
        public int? MerchantId { get; set; }
        public PaymentMethodODTO? PaymentMethod { get; set; }
        public bool IsActive { get; set; }
    }
}
