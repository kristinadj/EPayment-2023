namespace Base.DTO.Output
{
    public class PaymentInstructionsODTO
    {
        public int PaymentId { get; set; }
        public string PaymentUrl { get; set; }

        public PaymentInstructionsODTO(string paymentUrl)
        {
            PaymentUrl = paymentUrl;
        }
    }
}
