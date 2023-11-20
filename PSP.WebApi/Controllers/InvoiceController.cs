using AutoMapper;
using Base.DTO.Input;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PSP.WebApi.DTO.Output;
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

        public InvoiceController(
            IPaymentMethodService paymentMethodService,
            IMerchantService merchantService,
            IInvoiceService invoiceService,
            IConsulHttpClient consulHttpClient,
            IMapper mapper)
        {
            _paymentMethodService = paymentMethodService;
            _merchantService = merchantService;
            _invoiceService = invoiceService;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
        }

        [HttpPost("{paymentMethodId}")]
        public async Task<ActionResult<InvoiceODTO>> CreateInvoice([FromRoute] int paymentMethodId, [FromBody] PspInvoiceIDTO invoiceIDTO)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
            if (paymentMethod == null)  return BadRequest();

            var merchant = await _merchantService.GetMerchantByIdAsync(invoiceIDTO.MerchantId);
            if (merchant == null)  return NotFound();

            var paymentMethodCredentials = merchant.PaymentMethods!/*.Where(x => x.PaymentMethodId == paymentMethod.PaymentMethodId)*/.FirstOrDefault();
            if (paymentMethodCredentials == null) return NotFound();

            var invoice = await _invoiceService.CreateInvoiceAsync(merchant, paymentMethod, invoiceIDTO);
            if (invoice == null) return BadRequest();

            var paymentRequest = new PaymentRequestIDTO(paymentMethodCredentials.Secret, invoice.Currency!.Code, merchant.TransactionSuccessUrl, merchant.TransactionFailureUrl, merchant.TransactionErrorUrl)
            {
                MerchantId = paymentMethodCredentials.PaymentMethodMerchantId,
                Amount = invoiceIDTO.TotalPrice,
                ExternalInvoiceId = invoice.InvoiceId,
                Timestamp = invoiceIDTO.Timestamp,
                TransactionSuccessUrl = merchant.TransactionSuccessUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                TransactionFailureUrl = merchant.TransactionFailureUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                TransactionErrorUrl = merchant.TransactionErrorUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString())
            };

            var result = _mapper.Map<InvoiceODTO>(invoiceIDTO);
            try
            {
                var paymentInstructions = await _consulHttpClient.PostAsync(paymentMethod.ServiceName, $"{paymentMethod.ServiceApiSufix}/Invoice", paymentRequest);
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