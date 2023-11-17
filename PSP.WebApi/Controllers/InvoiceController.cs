using AutoMapper;
using Base.DTO.Shared;
using Base.Services.Clients;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PSP.WebApi.DTO.Input;
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
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;

        public InvoiceController(
            IPaymentMethodService paymentMethodService,
            IMerchantService merchantService,
            IConsulHttpClient consulHttpClient,
            IMapper mapper)
        {
            _paymentMethodService = paymentMethodService;
            _merchantService = merchantService;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
        }

        [HttpPost("{paymentMethodId}")]
        public async Task<ActionResult<InvoiceODTO>> CreateInvoice([FromRoute] int paymentMethodId, [FromBody] PspInvoiceIDTO invoiceIDTO)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
            if (paymentMethod == null) return BadRequest();

            var merchant = await _merchantService.GetMerchantByIdAsync(invoiceIDTO.MerchantId);
            if (merchant == null) return NotFound();

            var invoice = _mapper.Map<InvoiceODTO>(invoiceIDTO);
            try
            {
                await _consulHttpClient.GetAsync(paymentMethod.ServiceName, $"{paymentMethod.ServiceApiSufix}/Invoice");
                invoice.RedirectUrl = merchant.TransactionSuccessUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString());

                // TODO: Add for failure
            }
            catch (HttpRequestException)
            {
                invoice.RedirectUrl = merchant.TransactionErrorUrl.Replace("@INVOICE_ID@", invoice.ExternalInvoiceId.ToString());
            }

            return Ok(invoice);
        }
    }
}