namespace WebShop.WebApi.AppSettings
{
    public class PspAppSettings
    {
        public string ServiceName { get; set; }
        public string RegisterMerchantApiEndpoint { get; set; }

        public PspAppSettings()
        {
            ServiceName = string.Empty;
            RegisterMerchantApiEndpoint = string.Empty;
        }

        public PspAppSettings(string pspServiceName, string pspRegisterApiEndpoint)
        {
            ServiceName = pspServiceName;
            RegisterMerchantApiEndpoint = pspRegisterApiEndpoint;
        }
    }
}
