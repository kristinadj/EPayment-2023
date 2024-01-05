using Base.DTO.Input;
using Base.DTO.Output;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using PayPalPaymentService.WebApi.AppSettings;
using PayPalPaymentService.WebApi.DTO.PayPal.Input;
using PayPalPaymentService.WebApi.Enums;
using PayPalPaymentService.WebApi.Helpers;
using PayPalPaymentService.WebApi.Services;

namespace PayPalPaymentService.WebApi.Controllers
{
    [Route("api/paypal/[controller]")]
    [ApiController]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMerchantService _merchantService;

        private readonly IPayPalClientService _paytPalClientService;
        private readonly PayPalSettings _payPalSettings;

        private readonly PaymentMethod _paymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public SubscriptionPaymentController(
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
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateAutomaticPayment([FromBody] AutomaticPaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _merchantService.GetMerchantByPaymentServiceMerchantId(paymentRequestDTO.MerchantId);
            if (merchant == null) return BadRequest();

            var accessToken = await _paytPalClientService.GenerateAccessTokenAsync(merchant.ClientId, merchant.Secret);
            if (accessToken == null) return BadRequest();

            var createNewBillingPlan = false;

            if (merchant.PayPalBillingPlanProductId == null)
            {
                var productIDTO = Mapper.ToPayPalProductIDTO(paymentRequestDTO.Product!);
                var productODTO = await _paytPalClientService.CreateProductAsync(accessToken, productIDTO);

                if (productODTO == null) return BadRequest();

                merchant = await _merchantService.UpdateProductIdAsync(merchant, productODTO.Id!);
                createNewBillingPlan = true;
            }

            if (merchant.PayPalBillingPlanId == null || createNewBillingPlan )
            {
                var createPlanIDTO = Mapper.ToCreatePlanIDTO(merchant.PayPalBillingPlanProductId!, paymentRequestDTO.ExternalInvoiceId.ToString(), paymentRequestDTO.Amount, paymentRequestDTO.CurrencyCode);
                var billingPlanODTO = await _paytPalClientService.CreatePlanAsync(accessToken, createPlanIDTO);

                if (billingPlanODTO == null) return BadRequest();

                merchant = await _merchantService.UpdateBillingPlanIdAsync(merchant, billingPlanODTO.Id!);
            }

            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO, InvoiceType.SUBSCRIPTION);
            if (invoice == null) return BadRequest();

            var subscriptionIDTO = Mapper.ToCreateSubscriptionIDTO(merchant.PayPalBillingPlanId!, paymentRequestDTO.BrandName!, paymentRequestDTO.Subscriber!, _payPalSettings);

            var subscriptionResponse = await _paytPalClientService.CreateSubscriptionAsync(accessToken, subscriptionIDTO);
            if (subscriptionResponse == null)
            {
                await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.ERROR);
                return BadRequest();
            }

            await _invoiceService.UpdatePayPalSubscriptionIdAsync(invoice.InvoiceId, subscriptionResponse.Id!);
            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, TransactionStatus.IN_PROGRESS);

            var redirectUrl = subscriptionResponse.Links.Where(x => x.Rel == "approve").FirstOrDefault();
            var paymentInstructions = new PaymentInstructionsODTO(subscriptionResponse.Id!, redirectUrl!.Href);
            return Ok(paymentInstructions);
        }

        [HttpGet("Success")]
        public async Task<ActionResult> PayPalSubscriptionSuccess([FromQuery] string subscription_id)
        {
            var invoice = await _invoiceService.GetInvoiceByPayPalSubscriptionIdAsync(subscription_id);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, TransactionStatus.COMPLETED);
            //await _invoiceService.UpdatePayerIdAsync(invoice.InvoiceId, payerId);

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
        public async Task<ActionResult> PayPalSubscriptionCancel([FromQuery] string subscription_id)
        {
            var invoice = await _invoiceService.GetInvoiceByPayPalSubscriptionIdAsync(subscription_id);
            if (invoice == null) return BadRequest();

            await _invoiceService.UpdateInvoiceStatusAsync(invoice.InvoiceId, Enums.TransactionStatus.FAIL);

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
