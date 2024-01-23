using AutoMapper;
using Base.DTO.Input;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PSP.WebApi.AppSettings;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Models;
using PSP.WebApi.Services;

namespace PSP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class InvoiceController : ControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IMerchantService _merchantService;
        private readonly IInvoiceService _invoiceService;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;
        private readonly PspAppSettings _pspAppSettings;

        public InvoiceController(
            IPaymentMethodService paymentMethodService,
            IMerchantService merchantService,
            IInvoiceService invoiceService,
            IConsulHttpClient consulHttpClient,
            IMapper mapper,
            IOptions<PspAppSettings> pspAppSettings)
        {
            _paymentMethodService = paymentMethodService;
            _merchantService = merchantService;
            _invoiceService = invoiceService;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
            _pspAppSettings = pspAppSettings.Value;
        }

        [HttpGet("{invoiceId}")]
        public async Task<ActionResult<InvoiceODTO>> GetInvoiceById([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceByIdAsyync(invoiceId);
            if (invoice == null) return NotFound($"Invoice {invoiceId} doesn't exist");

            return Ok(invoice);
        }

        [HttpPost]
        public async Task<ActionResult<string>> CreateInvoice([FromBody] PspInvoiceIDTO invoiceIDTO)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(invoiceIDTO.MerchantId);
            if (merchant == null) return NotFound($"Merchant {invoiceIDTO.MerchantId} doesn't exist");

            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(merchant, invoiceIDTO, invoiceIDTO.InvoiceType);
                if (invoice == null) return BadRequest($"Invalid currency {invoiceIDTO.CurrencyCode}");

                var result = _mapper.Map<InvoiceODTO>(invoiceIDTO);
                result.RedirectUrl = $"{_pspAppSettings.ClientUrl}/paymentMethods/{invoice.InvoiceId}/false";
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("PaymentMethod/{invoiceId};{paymentMethodId}")]
        public async Task<ActionResult<RedirectUrlDTO>> UpdatePaymentMethod([FromRoute] int invoiceId, [FromRoute] int paymentMethodId)
        {
            var invoice = await _invoiceService.UpdatePaymentMethodAsync(invoiceId, paymentMethodId);
            if (invoice == null) return NotFound();

            var paymentMethodCredentials = invoice.Merchant!.PaymentMethods!.Where(x => x.PaymentMethodId == paymentMethodId).FirstOrDefault();
            if (paymentMethodCredentials == null) return NotFound();

            var result = new RedirectUrlDTO(string.Empty);

            // 1. Update WebShop Invoice with choosen payment method
            try
            {
                await _consulHttpClient.PutAsync(invoice.Merchant.ServiceName, $"api/Invoice/UpdatePaymentMethod/{invoice.ExternalInvoiceId};{paymentMethodId}");
            }
            catch (Exception)
            {
                //TODO: Update transaction status fail
                result.RedirectUrl = invoice.Merchant!.TransactionErrorUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString());
            }

            if (!invoice.RecurringPayment)
            {
                // 2. Initiate payment
                var paymentRequest = new PaymentRequestIDTO
                {
                    MerchantId = paymentMethodCredentials.MerchantId,
                    Amount = invoice.TotalPrice,
                    ExternalInvoiceId = invoice.InvoiceId,
                    Timestamp = invoice.Transaction!.CreatedTimestamp,
                    TransactionSuccessUrl = invoice.Merchant!.TransactionSuccessUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                    TransactionFailureUrl = invoice.Merchant!.TransactionFailureUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                    TransactionErrorUrl = invoice.Merchant!.TransactionErrorUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                    CurrencyCode = invoice.Currency!.Code,
                    InvoiceType = invoice.InvoiceType
                };

                try
                {
                    var paymentInstructions = await _consulHttpClient.PostAsync(invoice.Transaction.PaymentMethod!.ServiceName, $"{invoice.Transaction.PaymentMethod!.ServiceApiSufix}/Invoice", paymentRequest);
                    if (paymentInstructions != null)
                    {
                        result.RedirectUrl = paymentInstructions!.PaymentUrl;
                    }
                    else
                    {
                        //TODO: Update transaction status fail
                        result.RedirectUrl = paymentRequest.TransactionFailureUrl;
                    }
                }
                catch (HttpRequestException)
                {
                    result.RedirectUrl = paymentRequest.TransactionErrorUrl;
                }

                return Ok(result);
            }
            else
            {
                var subscriptionDetails = await _invoiceService.GetSubscriptioNdetailsByInvoiceIdAsync(invoice.InvoiceId);
                if (subscriptionDetails == null) return BadRequest();

                // 2. Initiate payment
                var paymentRequest = new RecurringPaymentRequestIDTO
                {
                    MerchantId = paymentMethodCredentials.MerchantId,
                    Amount = invoice.TotalPrice,
                    ExternalInvoiceId = invoice.InvoiceId,
                    Timestamp = invoice.Transaction!.CreatedTimestamp,
                    TransactionSuccessUrl = invoice.Merchant!.TransactionSuccessUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                    TransactionFailureUrl = invoice.Merchant!.TransactionFailureUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                    TransactionErrorUrl = invoice.Merchant!.TransactionErrorUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                    CurrencyCode = invoice.Currency!.Code,
                    InvoiceType = invoice.InvoiceType,
                    BrandName = subscriptionDetails.BrandName,
                    Product = new ProductIDTO(subscriptionDetails.ProductName!, subscriptionDetails.ProductType!, subscriptionDetails.ProductDescription!, subscriptionDetails.ProductCategory!),
                    Subscriber = new SubscriberIDTO
                    {
                        Email = subscriptionDetails.SubscriberEmail,
                        Name = subscriptionDetails.SubscriberName
                    },
                    RecurringTransactionSuccessUrl = subscriptionDetails.RecurringTransactionSuccessUrl,
                    RecurringTransactionFailureUrl = subscriptionDetails.RecurringTransactionFailureUrl
                };

                try
                {
                    var paymentInstructions = await _consulHttpClient.PostAsync(invoice.Transaction.PaymentMethod!.ServiceName, $"{invoice.Transaction.PaymentMethod!.ServiceApiSufix}/SubscriptionPayment", paymentRequest);
                    if (paymentInstructions != null)
                    {
                        result.RedirectUrl = paymentInstructions.PaymentUrl!;
                    }
                    else
                    {
                        //TODO: Update transaction status fail
                        result.RedirectUrl = paymentRequest.TransactionFailureUrl;
                    }
                }
                catch (HttpRequestException)
                {
                    result.RedirectUrl = paymentRequest.TransactionErrorUrl;
                }

                return Ok(result);
            }
        }

        [HttpPut("{invoiceId}/Success")]
        public async Task<ActionResult<RedirectUrlDTO>> SuccessPayment([FromRoute] int invoiceId)
        {
            var isSuccess = await _invoiceService.UpdateTransactionStatusAsync(invoiceId, Enums.TransactionStatus.COMPLETED);
            if (!isSuccess) return BadRequest();

            return Ok();
        }

        [HttpPut("{invoiceId}/Failure")]
        public async Task<ActionResult<RedirectUrlDTO>> FailurePayment([FromRoute] int invoiceId)
        {
            var isSuccess = await _invoiceService.UpdateTransactionStatusAsync(invoiceId, Enums.TransactionStatus.FAIL);
            if (!isSuccess) return BadRequest();

            return Ok();
        }

        [HttpPut("{invoiceId}/Error")]
        public async Task<ActionResult<RedirectUrlDTO>> ErrorPayment([FromRoute] int invoiceId)
        {
            var isSuccess = await _invoiceService.UpdateTransactionStatusAsync(invoiceId, Enums.TransactionStatus.ERROR);
            if (!isSuccess) return BadRequest();

            return Ok();
        }
    }
}