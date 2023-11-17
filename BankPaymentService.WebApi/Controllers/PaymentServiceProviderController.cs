﻿using BankPaymentService.WebApi.AppSettings;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServiceProviderController : ControllerBase
    {
        private readonly CardPaymentMethod _cardPaymentMethod;
        private readonly QrCodePaymentMethod _qrCodePaymentMethod;
        private readonly ConsulAppSettings _consulAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        public PaymentServiceProviderController(
            IOptions<CardPaymentMethod> cardPaymentMethod,
            IOptions<QrCodePaymentMethod> qrCodePaymentMethod,
            IOptions<ConsulAppSettings> consulAppSettings, 
            IConsulHttpClient consulHttpClient)
        {
            _cardPaymentMethod = cardPaymentMethod.Value;
            _qrCodePaymentMethod = qrCodePaymentMethod.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost("Register/Card")]
        public async Task<ActionResult<PaymentMethodDTO>> RegisterCard()
        {
            var paymentMethod = new PaymentMethodDTO(_cardPaymentMethod.Name, _consulAppSettings.Service, _cardPaymentMethod.ServiceApiSufix);
            var result = await _consulHttpClient.PostAsync(_cardPaymentMethod.PspServiceName, _cardPaymentMethod.PspRegisterApiEndpoint, paymentMethod);

            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("Register/QrCode")]
        public async Task<ActionResult<PaymentMethodDTO>> RegisterQrCode()
        {
            var paymentMethod = new PaymentMethodDTO(_qrCodePaymentMethod.Name, _consulAppSettings.Service, _qrCodePaymentMethod.ServiceApiSufix);
            var result = await _consulHttpClient.PostAsync(_qrCodePaymentMethod.PspServiceName, _qrCodePaymentMethod.PspRegisterApiEndpoint, paymentMethod);

            if (result == null) return BadRequest();

            return Ok(result);
        }
    }
}
