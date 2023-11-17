﻿using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace BankPaymentService.WebApi.Controllers
{
    [Route("api/qrcode/Invoice")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class InvoiceQrCodeController : ControllerBase
    {
        public InvoiceQrCodeController()
        {
        }

        [HttpGet]
        public ActionResult<RedirectUrlDTO> CreateInvoice()
        {
            var redirectUrl = new RedirectUrlDTO("Success QR");
            return Ok(redirectUrl);
        }
    }
}