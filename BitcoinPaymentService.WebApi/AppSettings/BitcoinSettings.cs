namespace BitcoinPaymentService.WebApi.AppSettings
{
    public class BitcoinSettings
    {
        public string SuccessUrl {  get; set; } = string.Empty;
        public string CancelUrl { get; set; } = string.Empty;
        public string CoingateUrl { get; set; } = string.Empty;
        public string CoingateKey { get; set; } = string.Empty; 
    }
}
