using System.Net.Http.Json;
using System.Text.Json;
using System.Text;
using WebShop.DTO.Input;
using WebShop.DTO.Output;
using Base.DTO.Shared;
using WebShop.DTO.Enums;

namespace WebShop.Client.Services
{
    public class ApiServices : IApiServices
    {
        private readonly HttpClient _httpClient;

        public ApiServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ItemODTO>> GetItemsAsync()
        {
            var data = new List<ItemODTO>();

            try
            {
                var response = await _httpClient.GetAsync("api/Items");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<List<ItemODTO>>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<ShoppingCartODTO?> GetShoppingCartByUserAsync(string userId)
        {
            ShoppingCartODTO? data = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/ShoppingCart/ByUser/{userId}");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<ShoppingCartODTO>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<bool> AddItemInShoppingCartAsync(ShoppingCartItemIDTO itemDTO)
        {
            var isSuccess = false;

            try
            {
                var content = new StringContent(JsonSerializer.Serialize(itemDTO), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/ShoppingCartItem", content);
                response.EnsureSuccessStatusCode();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return isSuccess;
        }

        public async Task<OrderODTO?> CreateOrderAsync(int shoppingCartId)
        {
            OrderODTO? data = null;

            try
            {
                var response = await _httpClient.PostAsync($"api/ShoppingCart/Checkout/{shoppingCartId}", null);
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<OrderODTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<RedirectUrlDTO?> CreateInvoiceAsync(int orderId, int paymentMethodId)
        {
            RedirectUrlDTO? data = null;

            try
            {
                var response = await _httpClient.PostAsync($"api/Invoice/{orderId};{paymentMethodId}", null);
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<RedirectUrlDTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<List<PaymentMethodODTO>> GetPaymentMethodsAsync()
        {
            var data = new List<PaymentMethodODTO>();

            try
            {
                var response = await _httpClient.GetAsync("api/PaymentMethod");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<List<PaymentMethodODTO>>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<OrderODTO?> CancelOrderAsync(int orderId)
        {
            OrderODTO? data = null;

            try
            {
                var response = await _httpClient.PutAsync($"api/Order/Cancel/{orderId}", null);
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<OrderODTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<OrderODTO?> GetOrderByIdAsync(int orderId)
        {
            OrderODTO? data = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Order/ById/{orderId}");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<OrderODTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId)
        {
            InvoiceODTO? data = null;

            try
            {
                var response = await _httpClient.GetAsync($"api/Invoice/ById/{invoiceId}");
                response.EnsureSuccessStatusCode();

                var tempData = await response.Content.ReadFromJsonAsync<InvoiceODTO?>();
                if (tempData != null) { data = tempData; }
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return data;
        }

        public async Task<bool> UpdateTransactionStatusAsync(int transactionid, TransactionStatus transactionStatus)
        {
            try
            {
                var response = await _httpClient.PutAsync($"api/Transaction/{transactionid};{transactionStatus}", null);
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (Exception ex)
            {
                // TODO:
            }

            return false;
        }
    }
}
