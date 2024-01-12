using Base.DTO.Input;
using PayPalPaymentService.WebApi.AppSettings;
using PayPalPaymentService.WebApi.DTO.PayPal.Input;
using PayPalPaymentService.WebApi.Models;

namespace PayPalPaymentService.WebApi.Helpers
{
    public static class Mapper
    {
        public static PayPalProductIDTO ToPayPalProductIDTO(ProductIDTO productIDTO)
        {
            return new PayPalProductIDTO(productIDTO.Name, productIDTO.Type, productIDTO.Description, productIDTO.Category);
        }

        public static OrderIDTO ToOrderIDTO(PaymentRequestIDTO paymentRequestDTO, Invoice invoice, PayPalSettings payPalSettings) 
        { 
            return new OrderIDTO
            {
                PurchaseUnits = new List<PurchaseUnitIDTO>
                {
                    new() {
                        Amount = new AmountIDTO
                        {
                            CurrencyCode = paymentRequestDTO.CurrencyCode,
                            Value = paymentRequestDTO.Amount.ToString()
                        },
                        ReferenceId = invoice.InvoiceId.ToString()
                    }
                },
                PaymentSource = new PaymentSourceIDTO
                {
                    Paypal = new PaypalIDTO
                    {
                        ExperienceContext = new ExperienceContextIDTO
                        {
                            ReturnUrl = payPalSettings.ReturnUrl,
                            CancelUrl = payPalSettings.CancelUrl,
                        }
                    }
                }
            };
        }

        public static CreatePlanIDTO ToCreatePlanIDTO(string productId, string name, double value, string currencyCode)
        {
            return new CreatePlanIDTO
            {
                ProductId = productId,
                Name = name,
                Status = "ACTIVE",
                BillingCycles = new List<BillingCycleIDTO>
                {
                    new BillingCycleIDTO
                    {
                        Frequency = new FrequencyIDTO
                        {
                             IntervalUnit = "YEAR",
                             IntervalCount = 1
                        },
                        TenureType = "REGULAR",
                        Sequence = 1,
                        TotalCycles = 0,
                        PricingScheme = new PricingSchemeIDTO
                        {
                            FixedPrice = new FixedPriceIDTO
                            {
                                Value = value.ToString(),
                                CurrencyCode = currencyCode
                            }
                        }
                    }
                },
                PaymentPreferences = new PaymentPreferencesIDTO
                {
                    AutoBillOutstanding = true,
                    PaymentFailureThreshold = 3
                }
            };
        }

        public static CreateSubscriptionIDTO ToCreateSubscriptionIDTO(string planId, string brandName, SubscriberIDTO subscriber, PayPalSettings payPalSettings)
        {
            return new CreateSubscriptionIDTO
            {
                PlanId = planId,
                Subscriber = new PayPalSubscriberIDTO
                {
                    Name = new PayPalNameIDTO
                    {
                        GivenName = subscriber.Name,
                        Surname = subscriber.LastName
                    },
                    EmailAddress = subscriber.Email
                },
                ApplicationContext = new ApplicationContextIDTO
                {
                    BrandName = brandName,
                    UserAction = "SUBSCRIBE_NOW",
                    PaymentMethod = new PaymentMethodIDTO
                    {
                        PayerSelected = "PAYPAL",
                        PayeePreferred = "IMMEDIATE_PAYMENT_REQUIRED"
                    },
                    ReturnUrl = payPalSettings.SubscriptionReturnUrl,
                    CancelUrl = payPalSettings.SubscriptionCancelUrl
                }
            };
        }
    }
}
