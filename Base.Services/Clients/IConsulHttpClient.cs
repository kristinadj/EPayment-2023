using Consul;
using Newtonsoft.Json;

namespace Base.Services.Clients
{
    public interface IConsulHttpClient
    {
        Task<T?> GetAsync<T>(string serviceName, Uri requestUri);
        Task<Dictionary<string, Uri>> GetServiceUrisByTagAsync(string tag);
    }

    public class ConsulHttpClient : IConsulHttpClient 
    {
        private readonly HttpClient _client;
        private IConsulClient _consulclient;

        public ConsulHttpClient(HttpClient client, IConsulClient consulclient)
        {
            _client = client;
            _consulclient = consulclient;
        }

        public async Task<T?> GetAsync<T>(string serviceName, Uri requestUri)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var response = await _client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        private async Task<Uri> GetRequestUriAsync(string serviceName, Uri uri)
        {
            //Get all services registered on Consul
            var allRegisteredServices = await _consulclient.Agent.Services();

            //Get all instance of the service went to send a request to
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            //Get a random instance of the service
            var service = GetRandomInstance(registeredServices!, serviceName);

            if (service == null)
            {
                throw new Exception($"Consul service: '{serviceName}' was not found.");
            }

            var uriBuilder = new UriBuilder(uri)
            {
                Host = service.Address,
                Port = service.Port
            };

            return uriBuilder.Uri;
        }

        public async Task<Dictionary<string, Uri>> GetServiceUrisByTagAsync(string tag)
        {
            var allRegisteredServices = await _consulclient.Agent.Services();

            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Tags.Contains(tag)).Select(x => x.Value).ToList();
            var groupedRegisteredServices = registeredServices!.GroupBy(x => x.Service).ToDictionary(x => x.Key, x => x.ToList());

            var result = new Dictionary<string, Uri>();

            foreach (var serviceName in groupedRegisteredServices.Keys)
            {
                var service = GetRandomInstance(groupedRegisteredServices[serviceName], serviceName);

                if (service == null)
                {
                    throw new Exception($"Consul service: '{serviceName}' was not found.");
                }

                var uriBuilder = new UriBuilder
                {
                    Host = service.Address,
                    Port = service.Port
                };

                result[serviceName] = uriBuilder.Uri;
            }

            return result;
        }

        private AgentService GetRandomInstance(IList<AgentService> services, string serviceName)
        {
            Random _random = new Random();

            AgentService? servToUse = null;

            servToUse = services[_random.Next(0, services.Count)];

            return servToUse;
        }
    }
}
