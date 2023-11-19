using System.Text;

namespace BankPaymentService.WebApi.AppSettings
{
    public class BankPaymentServiceUrl
    {
        public string BaseUrl { get; set; }

        public BankPaymentServiceUrl()
        {
            BaseUrl = string.Empty;
        }
    }
}
