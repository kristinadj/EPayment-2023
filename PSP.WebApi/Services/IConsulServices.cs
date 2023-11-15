using Base.Services.Clients;

namespace PSP.WebApi.Services
{
    public interface IConsulServices
    {
        Task<Dictionary<string, Uri>> GetPaymentServicesAsync();
    }

    public class ConsulServices : IConsulServices 
    {
        private readonly IConsulHttpClient _consulHttpClient;

        public ConsulServices(IConsulHttpClient consulHttpClient)
        {
            _consulHttpClient = consulHttpClient;
        }

        public Task<Dictionary<string, Uri>> GetPaymentServicesAsync()
        {
            return _consulHttpClient.GetServiceUrisByTagAsync("payment-service");
        }
    }
}
