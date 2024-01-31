namespace Base.Services.AppSettings
{
    public class PaymentMethod
    {
        public string PspServiceName { get; set; }
        public string PspRegisterApiEndpoint { get; set; }
        public string Name { get; set; }
        public string ServiceApiSufix { get; set; }
        public bool SupportsAutomaticPayments { get; set; }
        public string EncriptionKey { get; set; }

        public PaymentMethod()
        {
            PspServiceName = string.Empty;
            PspRegisterApiEndpoint = string.Empty;
            Name = string.Empty;
            ServiceApiSufix = string.Empty;
            EncriptionKey = string.Empty;
        }

        public PaymentMethod(string pspServiceName, string pspRegisterApiEndpoint, string name, string serviceApiSufix, string encriptionKey)
        {
            PspServiceName = pspServiceName;
            PspRegisterApiEndpoint = pspRegisterApiEndpoint;
            Name = name;
            ServiceApiSufix = serviceApiSufix;
            EncriptionKey = encriptionKey;
        }
    }
}
