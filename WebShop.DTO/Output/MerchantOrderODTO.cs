namespace WebShop.DTO.Output
{
    public class MerchantOrderODTO
    {
        public int MerchantOrderId { get; set; }
        public int OrderId { get; set; }

        public MerchantODTO? Merchant { get; set; }
        public InvoiceODTO? Invoice { get; set; }
        public List<OrderItemODTO>? OrderItems { get; set; }
    }
}
