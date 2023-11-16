namespace Base.Services.AppSettings
{
    public class PaymentMethod
    {
        public string PspServiceName { get; set; }
        public string PspRegisterApiEndpoint { get; set; }
        public string Name { get; set; }
        public string ServiceApiSufix { get; set; }

        public PaymentMethod()
        {
            PspServiceName = string.Empty;
            PspRegisterApiEndpoint = string.Empty;
            Name = string.Empty;
            ServiceApiSufix = string.Empty;
        }

        public PaymentMethod(string pspServiceName, string pspRegisterApiEndpoint, string name, string serviceApiSufix)
        {
            PspServiceName = pspServiceName;
            PspRegisterApiEndpoint = pspRegisterApiEndpoint;
            Name = name;
            ServiceApiSufix = serviceApiSufix;
        }
    }
}
