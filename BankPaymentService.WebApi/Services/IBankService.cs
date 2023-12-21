using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.DTO.Bank.Input;
using BankPaymentService.WebApi.Models;
using Base.DTO.Input;
using Base.DTO.Output;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BankPaymentService.WebApi.Services
{
    public interface IBankService
    {
        Task<PaymentInstructionsODTO?> SendInvoiceToBankAsync(Invoice invoice, PaymentRequestIDTO paymentRequestIDTO);
    }

    public class BankService : IBankService
    {
        private readonly BankPaymentServiceContext _context;

        private readonly BankPaymentServiceUrl _bankPaymentServiceUrl;

        public BankService(BankPaymentServiceContext context, IOptions<BankPaymentServiceUrl> bankPaymentServiceUrl)
        {
            _context = context;
            _bankPaymentServiceUrl = bankPaymentServiceUrl.Value;
        }

        public async Task<PaymentInstructionsODTO?> SendInvoiceToBankAsync(Invoice invoice, PaymentRequestIDTO paymentRequestIDTO)
        {
            var merchant = await _context.Merchants
                .Where(x => x.MerchantId == invoice.MerchantId)
                .Include(x => x.Bank)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (merchant == null || merchant.Bank == null) return null;

            var transaction = new TransactionIDTO(merchant.Secret, merchant.PreferredAccountNumber, paymentRequestIDTO.CurrencyCode)
            {
                SenderId = merchant.BankMerchantId,
                Amount = paymentRequestIDTO.Amount,
                ExternalInvoiceId = invoice.InvocieId,
                Timestamp = paymentRequestIDTO.Timestamp,
                TransactionSuccessUrl = $"{_bankPaymentServiceUrl.BaseUrl}/api/card/Invoice/{invoice.InvocieId}/Success",
                TransactionFailureUrl = $"{_bankPaymentServiceUrl.BaseUrl}/api/card/Invoice/{invoice.InvocieId}/Failure",
                TransactionErrorUrl = $"{_bankPaymentServiceUrl.BaseUrl}/api/card/Invoice/{invoice.InvocieId}/Error"
            };

            try
            {
                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync($"{merchant.Bank.RedirectUrl}/Transaction", transaction);

                if (!response.IsSuccessStatusCode) return null;

                var content = await response.Content.ReadAsStringAsync();
                if (content == null) return default;

                return JsonSerializer.Deserialize<PaymentInstructionsODTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
    }
}
