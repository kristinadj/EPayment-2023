namespace PSP.WebApi.DTO.Output
{
    public class PaymentMethodODTO
    {
        public int PaymentMethodId { get; set; }
        public string Name { get; set; }
        public string ServiceName { get; set; }
        public string ServiceApiSufix { get; set; }

        public PaymentMethodODTO(string name, string serviceName, string serviceApiSufix)
        {
            Name = name;
            ServiceName = serviceName;
            ServiceApiSufix = serviceApiSufix;
        }
    }
}
