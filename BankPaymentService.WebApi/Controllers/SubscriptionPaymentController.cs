﻿using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/card/SubscriptionPayment")]
    [ApiController]
    public class SubscriptionPaymentController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMerchantService _merchantService;
        private readonly IBankService _bankService;

        private readonly PaymentMethod _paymentMethod;
        private readonly IConsulHttpClient _consulHttpClient;

        public SubscriptionPaymentController(
            IInvoiceService invoiceService,
            IMerchantService merchantService,
            IBankService bankService,
            IOptions<PaymentMethod> paymetnMethod,
            IConsulHttpClient consulHttpClient)
        {
            _invoiceService = invoiceService;
            _merchantService = merchantService;
            _bankService = bankService;
            _paymentMethod = paymetnMethod.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost]
        public async Task<ActionResult> CreateRecurringPayment([FromBody] RecurringPaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _merchantService.GetMerchantByPaymentServiceMerchantId(paymentRequestDTO.MerchantId);
            if (merchant == null) return BadRequest();

            var invoice = await _invoiceService.CreateInvoiceAsync(paymentRequestDTO);
            if (invoice == null) return BadRequest();

            var paymentInstructions = await _bankService.SendRecurringPaymentToBankAsync(invoice, paymentRequestDTO);
            if (paymentInstructions == null) return BadRequest();

            return Ok(paymentInstructions);
        }
    }
}
