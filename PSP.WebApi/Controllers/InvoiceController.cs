﻿using AutoMapper;
using Base.DTO.Input;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PSP.WebApi.AppSettings;
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
            if (invoice == null) return NotFound();

            return Ok(invoice);
        }

        [HttpPost("{paymentMethodId}")]
        public async Task<ActionResult<InvoiceODTO>> CreateInvoiceV1([FromRoute] int paymentMethodId, [FromBody] PspInvoiceIDTO invoiceIDTO)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
            if (paymentMethod == null) return BadRequest();

            var merchant = await _merchantService.GetMerchantByIdAsync(invoiceIDTO.MerchantId);
            if (merchant == null) return NotFound();

            var paymentMethodCredentials = merchant.PaymentMethods!/*.Where(x => x.PaymentMethodId == paymentMethod.PaymentMethodId)*/.FirstOrDefault();
            if (paymentMethodCredentials == null) return NotFound();

            var invoice = await _invoiceService.CreateInvoiceAsync(merchant, paymentMethod, invoiceIDTO);
            if (invoice == null) return BadRequest();

            var paymentRequest = new PaymentRequestIDTO(paymentMethodCredentials.Secret, invoice.Currency!.Code)
            {
                Code = paymentMethodCredentials.Code,
                MerchantId = paymentMethodCredentials.MerchantId,
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

        [HttpPost]
        public async Task<ActionResult<string>> CreateInvoice([FromBody] PspInvoiceIDTO invoiceIDTO)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(invoiceIDTO.MerchantId);
            if (merchant == null)  return NotFound();

            var invoice = await _invoiceService.CreateInvoiceAsync(merchant, invoiceIDTO);
            if (invoice == null) return BadRequest();

            var result = _mapper.Map<InvoiceODTO>(invoiceIDTO);
            result.RedirectUrl = $"{_pspAppSettings.ClientUrl}/paymentMethods/{invoice.InvoiceId}";
            return Ok(result);
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

            // 2. Initiate payment
            var paymentRequest = new PaymentRequestIDTO(paymentMethodCredentials.Secret, invoice.Currency!.Code)
            {
                Code = paymentMethodCredentials.Code,
                MerchantId = paymentMethodCredentials.MerchantId,
                Amount = invoice.TotalPrice,
                ExternalInvoiceId = invoice.InvoiceId,
                Timestamp = invoice.Transaction!.CreatedTimestamp,
                TransactionSuccessUrl = invoice.Merchant!.TransactionSuccessUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                TransactionFailureUrl = invoice.Merchant!.TransactionFailureUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString()),
                TransactionErrorUrl = invoice.Merchant!.TransactionErrorUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString())
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