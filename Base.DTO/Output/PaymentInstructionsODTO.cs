namespace Base.DTO.Output
{
    public class PaymentInstructionsODTO
    {
        public string PaymentId { get; set; }
        public string PaymentUrl { get; set; }

        public PaymentInstructionsODTO(string paymentId, string paymentUrl)
        {
            PaymentId = paymentId;
            PaymentUrl = paymentUrl;
        }
    }
}
