using Base.DTO.Input;
using Base.DTO.Output;
using Base.DTO.Shared;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PayPalPaymentService.WebApi.AppSettings;
using PayPalPaymentService.WebApi.DTO.PayPal.Input;
using PayPalPaymentService.WebApi.Services;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/paypal/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMerchantService _merchantService;

        private readonly IPayPalClientService _paytPalClientService;
        private readonly PayPalSettings _payPalSettings;

        private readonly PaymentMethod _paymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public InvoiceController(
            IInvoiceService invoiceService,
            IMerchantService merchantService,
            IPayPalClientService paytPalClientService, 
            IOptions<PayPalSettings> payPalSettings,
            IOptions<PaymentMethod> paymetnMethod,
            IConsulHttpClient consulHttpClient)
        {
            _invoiceService = invoiceService;
            _merchantService = merchantService;
            _paytPalClientService = paytPalClientService;
            _paymentMethod = paymetnMethod.Value;
            _payPalSettings = payPalSettings.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateInvoice([FromBody] PaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _merchantService.GetMerchantByPaymentServiceMerchantId(paymentRequestDTO.MerchantId);
            if (merchant == null) return BadRequest();

            var accessToken = await _paytPalClientService.GenerateAccessTokenAsync(merchant.ClientId, merchant.Secret);
            if (accessToken == null) return BadRequest();

            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var orderIDTO = new OrderIDTO
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
                            ReturnUrl = _payPalSettings.ReturnUrl,
                            CancelUrl = _payPalSettings.CancelUrl,
                        }
                    }
                }
            };

            var orderResponse = await _paytPalClientService.CreateOrderAsync(accessToken, orderIDTO); 
            if (orderResponse == null || orderResponse.Status != "PAYER_ACTION_REQUIRED")
            {
               await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.ERROR);
                return BadRequest();
            }

            await _invoiceService.UpdatePayPalOrderIdAsync(invoice.InvoiceId, orderResponse!.Id);
            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.IN_PROGRESS);

            var redirectUrl = orderResponse.Links.Where(x => x.Rel == "payer-action").FirstOrDefault();
            var paymentInstructions = new PaymentInstructionsODTO(redirectUrl!.Href);

            return Ok(paymentInstructions);
        }

        [HttpGet("Success")]
        public async Task<ActionResult> PayPalSuccess([FromQuery] string token, [FromQuery] string payerId)
        {
            var invoice = await _invoiceService.GetInvoiceByPayPalOrderIdAsync(token);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.COMPLETED);
            await _invoiceService.UpdatePayerIdAsync(invoice.InvoiceId, payerId);

            try
            {
                await _consulHttpClient.PutAsync(_paymentMethod.PspServiceName, $"{invoice!.ExternalInvoiceId}/Success");
                return Redirect(invoice.TransactionSuccessUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Cancel")]
        public async Task<ActionResult> PayPalCancel([FromQuery] string token, string payerId)
        {
            var invoice = await _invoiceService.GetInvoiceByPayPalOrderIdAsync(token);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.FAIL);
            await _invoiceService.UpdatePayerIdAsync(invoice.InvoiceId, payerId);

            try
            {
                await _consulHttpClient.PutAsync(_paymentMethod.PspServiceName, $"{invoice!.ExternalInvoiceId}/Failure");
                return Redirect(invoice.TransactionFailureUrl);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
