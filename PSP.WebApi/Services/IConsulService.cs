using Base.Services.Clients;

namespace PSP.WebApi.Services
{
    public interface IConsulService
    {
        Task<Dictionary<string, Uri>> GetPaymentServicesAsync();
    }

    public class ConsulService : IConsulService 
    {
        private readonly IConsulHttpClient _consulHttpClient;

        public ConsulService(IConsulHttpClient consulHttpClient)
        {
            _consulHttpClient = consulHttpClient;
        }

        public Task<Dictionary<string, Uri>> GetPaymentServicesAsync()
        {
            return _consulHttpClient.GetServiceUrisByTagAsync("payment-service");
        }
    }
}
