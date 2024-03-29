﻿using BankPaymentService.WebApi.AppSettings;
using BankPaymentService.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.Shared;
using Base.Services.AppSettings;
using Base.Services.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/qrcode/PaymentServiceProvider")]
    [ApiController]
    public class PaymentServiceProviderQrCodeController : ControllerBase
    {
        private readonly QrCodePaymentMethod _qrCodePaymentMethod;
        private readonly ConsulAppSettings _consulAppSettings;
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMerchantService _merchantService;

        public PaymentServiceProviderQrCodeController(
            IOptions<QrCodePaymentMethod> qrCodePaymentMethod,
            IOptions<ConsulAppSettings> consulAppSettings,
            IConsulHttpClient consulHttpClient,
            IMerchantService merchantService)
        {
            _qrCodePaymentMethod = qrCodePaymentMethod.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
            _merchantService = merchantService;
        }


        [HttpPost("Register")]
        public async Task<ActionResult<PaymentMethodDTO>> RegisterQrCode()
        {
            var paymentMethod = new PaymentMethodDTO(_qrCodePaymentMethod.Name, _consulAppSettings.Service, _qrCodePaymentMethod.ServiceApiSufix)
            {
                SupportsAutomaticPayments = _qrCodePaymentMethod.SupportsAutomaticPayments
            };
            var result = await _consulHttpClient.PostAsync(_qrCodePaymentMethod.PspServiceName, _qrCodePaymentMethod.PspRegisterApiEndpoint, paymentMethod);

            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpPut("Merchant/UpdateCredentials")]
        public async Task<ActionResult<PaymentMethodDTO>> UpdateMerchantCredentials(UpdateMerchantCredentialsIDTO updateMerchantCredentialsIDTO)
        {
            if (updateMerchantCredentialsIDTO.InstitutionId == null) return BadRequest("Isntitutionid is null");

            var isSuccess = await _merchantService.UpdateMerchantCredentialsAsync(updateMerchantCredentialsIDTO);
            if (!isSuccess) return BadRequest();

            return Ok();
        }
    }
}
