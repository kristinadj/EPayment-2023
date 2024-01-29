using Stripe;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;

namespace BitcoinPaymentService.WebApi.BitcoinClients
{
    public static class StripeClient
    {
        public async static Task<Session> CreateInvoiceAsync(string token, int invoiceId, decimal amount, string currencyCode, string successUrl, string cancelUrl)
        {
            //StripeConfiguration.ApiKey = token;
            StripeConfiguration.ApiKey = "sk_test_51OdIrpIEpErHnkt82qg1SDTAzJrSmUcC6XXUvePsUsnSx2c9FeCrp8LUbcvS4soezmA8OwLrUITgcdIVJrrR46yD00chUUqu7w";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "bitcoin"
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new() {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = currencyCode,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = invoiceId.ToString(),
                            },
                            UnitAmount = (long)amount,
                        },
                        Quantity = 1,
                    },
                },
                Mode = "payment",
                SuccessUrl = successUrl,
                CancelUrl = cancelUrl,
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }
    }
}
