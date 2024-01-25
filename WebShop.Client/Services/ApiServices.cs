using Base.DTO.Output;
using Base.DTO.Shared;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Json;
using WebShop.DTO.Enums;
using WebShop.DTO.Input;
using WebShop.DTO.Output;

namespace WebShop.Client.Services
{
    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ApiServices(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("WebShopAPI");
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<List<ItemODTO>> GetItemsAsync()
        {
            var data = new List<ItemODTO>();

            var response = await _httpClient.GetAsync("api/Items");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<ItemODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId)
        {
            ShoppingCartODTO? data = null;

            var response = await _httpClient.GetAsync($"api/ShoppingCart/ByUser/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<ShoppingCartODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(itemDTO), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/ShoppingCartItem", content);

            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task<bool> DeleteItemInShoppingCartAsync(int shoppingCartItemId)
        {
            var response = await _httpClient.DeleteAsync($"api/ShoppingCartItem/{shoppingCartItemId}");

            if (response.IsSuccessStatusCode) return true;
            return false;
        }

        public async Task<OrderODTO?> CreateOrderAsync(int shoppingCartId)
        {
            OrderODTO? data = null;

            var response = await _httpClient.PostAsync($"api/ShoppingCart/Checkout/{shoppingCartId}", null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<OrderODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<RedirectUrlDTO?> CreateInvoiceAsync(int orderId)
        {
            RedirectUrlDTO? data = null;

            var response = await _httpClient.PostAsync($"api/Invoice/{orderId}", null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<RedirectUrlDTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync()
        {
            var data = new List<PaymentMethodODTO>();

            var response = await _httpClient.GetAsync("api/PaymentMethod");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<PaymentMethodODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<OrderODTO?> CancelOrderAsync(int orderId)
        {
            OrderODTO? data = null;

            var response = await _httpClient.PutAsync($"api/Order/Cancel/{orderId}", null);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<OrderODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<OrderODTO?> GetOrderByIdAsync(int orderId)
        {
            OrderODTO? data = null;

            var response = await _httpClient.GetAsync($"api/Order/ById/{orderId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<OrderODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<OrderODTO?> GetOrderByInvoiceIdAsync(int invoiceId)
        {
            OrderODTO? data = null;

            var response = await _httpClient.GetAsync($"api/Order/ByInvoiceId/{invoiceId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<OrderODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId)
        {
            InvoiceODTO? data = null;

            var response = await _httpClient.GetAsync($"api/Invoice/ById/{invoiceId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<InvoiceODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<OrderODTO?> GeOrderByInvoiceIdAsync(int invoiceId)
        {
            OrderODTO? data = null;

            var response = await _httpClient.GetAsync($"api/Order/ByInvoiceId/{invoiceId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<OrderODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<bool> UpdateTransactionStatusAsync(int transactionid, TransactionStatus transactionStatus)
        {
            var response = await _httpClient.PutAsync($"api/Transaction/{transactionid};{transactionStatus}", null);
            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public async Task<List<SubscriptionPlanODTO>> GetSubscriptionPlansAsync()
        {
            var data = new List<SubscriptionPlanODTO>();

            var response = await _httpClient.GetAsync($"api/SubscriptionPlans");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<SubscriptionPlanODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<List<PaymentMethodMerchantODTO>> GetPaymentMethodsByUserIdAsync(string userId)
        {
            var data = new List<PaymentMethodMerchantODTO>();

            var response = await _httpClient.GetAsync($"api/PaymentServiceProvider/PaymentMethods/ByMerchantId/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<PaymentMethodMerchantODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<bool> SubscribeToPaymentMethodAsync(PaymentMethodSubscribeIDTO paymentMethodSubscribeIDTO)
        {
            var content = new StringContent(JsonSerializer.Serialize(paymentMethodSubscribeIDTO), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/PaymentServiceProvider/PaymentMethods/Subscribe", content);
            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public async Task<bool> UnsubscribeFromPaymentMethodAsync(int paymentMethodId, string userId)
        {
            var response = await _httpClient.PutAsync($"api/PaymentServiceProvider/PaymentMethods/Unsubscribe/{paymentMethodId};{userId}", null);
            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public async Task<bool> IsMerchantRegisteredOnPspAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"api/PaymentServiceProvider/Merchant/IsRegistered/{userId}");
            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public async Task<bool> RegisterMerchantOnPspAsync(string userId)
        {
            var response = await _httpClient.PostAsync($"api/PaymentServiceProvider/Merchant/Register/{userId}", null);
            if (response.IsSuccessStatusCode) return true;

            return false;
        }

        public async Task<bool> IsSubscriptionPlanValidAsync(string userId)
        {
            var response = await _httpClient.GetAsync($"api/SubscriptionPlans/IsValid/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    return JsonSerializer.Deserialize<bool>(content, _jsonSerializerOptions);
                }
            }

            return false;
        }

        public async Task<UserSubscriptionPlanDetailsODTO?> GetSubscriptionPlanDetailsAsync(string userId)
        {
            UserSubscriptionPlanDetailsODTO? data = null;

            var response = await _httpClient.GetAsync($"api/SubscriptionPlans/Details/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<UserSubscriptionPlanDetailsODTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<RedirectUrlDTO?> ChooseSubscriptionPlanAsync(UserSubscriptionPlanIDTO userSubscriptionPlanIDTO)
        {
            RedirectUrlDTO? data = null;

            var requestContent = new StringContent(JsonSerializer.Serialize(userSubscriptionPlanIDTO), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/SubscriptionPlans/Choose", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<RedirectUrlDTO>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task UpdateExternalSubscriptionIdAsync(int invoiceId, string externalSubscriptionId)
        {
            var queryString = new Dictionary<string, string?>
            {
                ["invoiceId"] = invoiceId.ToString(),
                ["externalSubscriptionId"] = externalSubscriptionId
            };

            var requestUri = QueryHelpers.AddQueryString($"api/SubscriptionPlans/ExternalSubscriptionId", queryString);
            await _httpClient.PutAsync(requestUri, null);
        }

        public async Task<bool> CancelSubscriptionAsync(string userId)
        {
            var response = await _httpClient.PutAsync($"api/SubscriptionPlans/CancelSubscription/{userId}", null);
            if (!response.IsSuccessStatusCode) return false;

            return true;
        }

        public async Task<List<InstitutionODTO>> GetPaymentMethodInstitutionsAsync(int paymentMethodId)
        {
            List<InstitutionODTO>? data = new();

            var response = await _httpClient.GetAsync($"api/PaymentServiceProvider/Institutions/{paymentMethodId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<InstitutionODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<List<InvoiceODTO>> GetBuyerInvoicesAsync(string userId)
        {
            List<InvoiceODTO> data = new();

            var response = await _httpClient.GetAsync($"api/Invoice/ByBuyerId/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<InvoiceODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }

        public async Task<List<InvoiceODTO>> GetMerchantInvoicesAsync(string userId)
        {
            List<InvoiceODTO> data = new();

            var response = await _httpClient.GetAsync($"api/Invoice/ByMerchantId/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var tempData = JsonSerializer.Deserialize<List<InvoiceODTO>>(content, _jsonSerializerOptions);
                    if (tempData != null) { data = tempData; }
                }
            }

            return data;
        }
    }
}
