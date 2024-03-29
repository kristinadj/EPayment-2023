﻿using Base.DTO.Input;
using Base.DTO.Output;
using Consul;
using System.Text;
using System.Text.Json;

namespace Base.Services.Clients
{
    public interface IConsulHttpClient
    {
        Task GetAsync(string serviceName, string requestUri);
        Task<T?> GetAsync<T>(string serviceName, string requestUri);
        Task<bool> PutAsync(string serviceName, string requestUri);
        Task<bool> PutAsync<T>(string serviceName, string requestUri, T requestBody);
        Task<T?> PostAsync<T>(string serviceName, string requestUri, T requestBody);
        Task<PaymentInstructionsODTO?> PostAsync(string serviceName, string requestUri, PaymentRequestIDTO requestBody);
        Task<bool> PostAsync(string serviceName, string requestUri);
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

        public async Task GetAsync(string serviceName, string requestUri)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var response = await _client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
        }

        public async Task<T?> GetAsync<T>(string serviceName, string requestUri)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var response = await _client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return default;
            }

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        public async Task<bool> PutAsync(string serviceName, string requestUri)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);
            var response = await _client.PutAsync(uri, null);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<bool> PutAsync<T>(string serviceName, string requestUri, T requestBody)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _client.PutAsync(uri, requestContent);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<T?> PostAsync<T>(string serviceName, string requestUri, T requestBody)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(uri, requestContent);

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            if (content == null) return default;

            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
        }

        public async Task<bool> PostAsync(string serviceName, string requestUri)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);
            var response = await _client.PostAsync(uri, null);

            if (!response.IsSuccessStatusCode) return false;
            return true;
        }

        public async Task<PaymentInstructionsODTO?> PostAsync(string serviceName, string requestUri, PaymentRequestIDTO requestBody)
        {
            var uri = await GetRequestUriAsync(serviceName, requestUri);

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync(uri, requestContent);

            if (!response.IsSuccessStatusCode) return default;

            var content = await response.Content.ReadAsStringAsync();
            if (content == null) return default;

            return JsonSerializer.Deserialize<PaymentInstructionsODTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task<Uri> GetRequestUriAsync(string serviceName, string uri)
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

            var uriBuilder = new UriBuilder
            {
                Host = service.Address,
                Port = service.Port,
                Path = uri,
            };

            return uriBuilder.Uri;
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
