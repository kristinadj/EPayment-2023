namespace PSP.Client.DTO.Output
{
    public class PaymentMethodODTO
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ServiceName { get; set; } = string.Empty;
        public string ServiceApiSufix { get; set; } = string.Empty;
    }
}
