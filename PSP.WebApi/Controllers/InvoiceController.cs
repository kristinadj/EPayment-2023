using AutoMapper;
using Base.Services.Clients;
using Base.DTO.Shared;
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
        private readonly IConsulHttpClient _consulHttpClient;

        private readonly IMapper _mapper;

        public InvoiceController(
            IPaymentMethodService paymentMethodService,
            IConsulHttpClient consulHttpClient,
            IMapper mapper)
        {
            _paymentMethodService = paymentMethodService;
            _consulHttpClient = consulHttpClient;
            _mapper = mapper;
        }

        [HttpPost("{paymentMethodId}")]
        public async Task<ActionResult<InvoiceODTO>> CreateInvoice([FromRoute] int paymentMethodId, [FromBody] InvoiceIDTO invoiceIDTO)
        {
            var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
            if (paymentMethod == null) return NotFound();

            var result = await _consulHttpClient.GetAsync<RedirectUrlDTO>(paymentMethod.ServiceName, $"{paymentMethod.ServiceApiSufix}/Invoice");

            if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
            {
                return BadRequest();
            }

            var invoice = _mapper.Map<InvoiceODTO>(invoiceIDTO);
            invoice.RedirectUrl = result.RedirectUrl;
            return Ok(invoice);
        }
    }
}