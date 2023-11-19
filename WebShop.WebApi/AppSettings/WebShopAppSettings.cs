namespace WebShop.WebApi.AppSettings
{
    public class WebShopAppSettings
    {
        public  string ClientUrl { get; set; }
        public WebShopAppSettings()
        {
            ClientUrl = string.Empty;
        }
    }
}
