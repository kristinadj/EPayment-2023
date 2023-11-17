namespace Base.DTO.Shared
{
    public class PaymentMethodDTO
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public string ServiceApiSufix { get; set; }

        public PaymentMethodDTO(string name, string serviceName, string serviceApiSufix)
        {
            Name = name;
            ServiceName = serviceName;
            ServiceApiSufix = serviceApiSufix;
        }
    }
}
