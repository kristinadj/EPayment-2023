using BitPay;
using BitPay.Models.Invoice;
using Environment = BitPay.Environment;

namespace BitcoinPaymentService.WebApi.BitcoinClients
{
    public static class BitPayClient
    {
        public async static Task<Invoice> GetInvoiceAsync(string token, string invoiceId)
        {
            var posToken = new PosToken(token);
            var bitPayClient = new Client(posToken, Environment.Test);
            return await bitPayClient.GetInvoice(invoiceId);
        }

        public async static Task<Invoice> CreateInvoiceAsync(string token, int invoiceId, decimal amount, string currencyCode, string successUrl, string cancelUrl)
        {
            var posToken = new PosToken(token);
            var bitPayClient = new Client(posToken, Environment.Test);

            var bitPayInvoice = new Invoice(amount, currencyCode)
            {
                OrderId = invoiceId.ToString(),
                RedirectUrl = successUrl,
                CloseURL = cancelUrl,
                Physical = false
            };

            return await bitPayClient.CreateInvoice(bitPayInvoice);
        }
    }
}
