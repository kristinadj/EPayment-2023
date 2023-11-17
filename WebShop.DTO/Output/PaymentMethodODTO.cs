namespace WebShop.DTO.Output
{
    public class PaymentMethodODTO
    {
        public int PaymentMethodId { get; set; }

        public int PspPaymentMethodId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public PaymentMethodODTO(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
