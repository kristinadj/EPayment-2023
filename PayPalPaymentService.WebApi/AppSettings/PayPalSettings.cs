namespace PayPalPaymentService.WebApi.AppSettings
{
    public class PayPalSettings
    {
        public string Url { get; set; } = string.Empty;
        public string ReturnUrl { get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
    }
}
