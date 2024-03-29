﻿using AutoMapper;
using Base.DTO.Shared;
using Base.Services.Clients;
using Base.DTO.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebShop.DTO.Enums;
using WebShop.DTO.Output;
using WebShop.WebApi.AppSettings;
using WebShop.WebApi.Services;

namespace WebShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IPspApiHttpClient _pspApiHttpClient;

        private readonly IMapper _mapper;

        public InvoiceController(
            IInvoiceService invoiceService,
            IPaymentMethodService paymentMethodService,
            IPspApiHttpClient pspApiHttpClient,
            IMapper mapper)
        {
            _invoiceService = invoiceService;
            _pspApiHttpClient = pspApiHttpClient;
            _mapper = mapper;
        }

        [HttpGet("ById/{invoiceId}")]
        public async Task<ActionResult<InvoiceODTO?>> GetById([FromRoute] int invoiceId)
        {
            try
            {
                var result = await _invoiceService.GetInvoiceByIdAsync(invoiceId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{orderId}")]
        public async Task<ActionResult<RedirectUrlDTO>> CreateInvoice([FromRoute] int orderId)
        {
            try
            {
                var invoice = await _invoiceService.CreateInvoiceAsync(orderId);

                if (invoice == null) return BadRequest();

                await _invoiceService.UpdateInvoiceTransactionStatusasync(invoice.InvoiceId, TransactionStatus.IN_PROGRESS);

                var pspPayment = _mapper.Map<PspInvoiceIDTO>(invoice);
                pspPayment.InvoiceType = InvoiceType.ORDER;

                var result = await _pspApiHttpClient.PostAsync($"Invoice", pspPayment);

                if (result == null || string.IsNullOrEmpty(result.RedirectUrl))
                {
                    return Ok(new RedirectUrlDTO($"/invoice/{invoice.InvoiceId}/error"));
                }

                return Ok(new RedirectUrlDTO(result!.RedirectUrl));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdatePaymentMethod/{invoiceId};{pspPaymentMethodId}")]
        [AllowAnonymous]
        public async Task<ActionResult<RedirectUrlDTO>> UpdatePaymentMethod([FromRoute] int invoiceId, [FromRoute] int pspPaymentMethodId)
        {
            try
            {
                await _invoiceService.UpdatePaymentMethodAsync(invoiceId, pspPaymentMethodId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ByBuyerId/{userId}")]
        public async Task<ActionResult<List<InvoiceODTO>>> GetBuyerInvoices([FromRoute] string userId)
        {
            try
            {
                var invoices = await _invoiceService.GetInvoicesByBuyerIdAsync(userId);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ByMerchantId/{userId}")]
        public async Task<ActionResult<List<InvoiceODTO>>> GetMerchantInvoices([FromRoute] string userId)
        {
            try
            {
                var invoices = await _invoiceService.GetInvoicesByMerchantIdAsync(userId);
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
