using Base.Services.AppSettings;
using Base.Services.Clients;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebShop.DTO.Output;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Models;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentServiceProviderController : ControllerBase
    {
        private readonly PspAppSettings _pspAppSettings;
        private readonly ConsulAppSettings _consulAppSettings;

        private readonly IMerchantService _merchantService;
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly IConsulHttpClient _consulHttpClient;

        public PaymentServiceProviderController(
            IMerchantService merchantService,
            IPaymentMethodService paymentMethodService,
            IOptions<PspAppSettings> pspAppSettings,
            IOptions<ConsulAppSettings> consulAppSettings,
            IConsulHttpClient consulHttpClient)
        {
            _merchantService = merchantService;
            _paymentMethodService = paymentMethodService;
            _pspAppSettings = pspAppSettings.Value;
            _consulAppSettings = consulAppSettings.Value;
            _consulHttpClient = consulHttpClient;
        }

        [HttpPost("Register/{merchantId}")]
        public async Task<ActionResult<MerchantDTO>> Register([FromRoute] int merchantId)
        {
            var merchant = await _merchantService.GetMerchantByIdAsync(merchantId);
            if (merchant == null) return NotFound();

            var merchantDTO = new MerchantDTO(merchant.User!.Name, _consulAppSettings.Service, "/invoice/@INVOICE_ID@/success", "/invoice/@INVOICE_ID@/failure", "/invoice/@INVOICE_ID@/error")
            {
                MerchantExternalId = merchant.MerchantId
            };
            var result = await _consulHttpClient.PostAsync(_pspAppSettings.ServiceName, _pspAppSettings.RegisterMerchantApiEndpoint, merchantDTO);

            if (result == null) return BadRequest();

            var isSuccess = await _merchantService.UpdatePspMerchantId(merchant.MerchantId, result.MerchantId);
            if (!isSuccess) return NotFound();

            return Ok(result);
        }

        [HttpGet("PaymentMethods")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> GetPaymentMethods()
        {
            var result = await _consulHttpClient.GetAsync<List<PaymentMethodDTO>>(_pspAppSettings.ServiceName, "/api/PaymentMethod");
            if (result == null) return BadRequest();

            return Ok(result);
        }

        [HttpPost("PaymentMethods/Import")]
        public async Task<ActionResult<List<PaymentMethodDTO>>> ImportPaymentMethods()
        {
            var result = await _consulHttpClient.GetAsync<List<PaymentMethodDTO>>(_pspAppSettings.ServiceName, "/api/PaymentMethod");
            if (result == null) return BadRequest();

            await _paymentMethodService.ImportFromPspAsync(result);

            return Ok(result);
        }
    }
}
