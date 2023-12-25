using Bank1.WebApi.AppSettings;
using Bank1.WebApi.DTO.Input;
using Bank1.WebApi.Helpers;
using Bank1.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.NBS;
using Base.DTO.Output;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank1.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IAccountService _accountService;
        private readonly BankSettings _appSettings;

        private readonly INbsClient _nbsClient;

        public TransactionController(
            ITransactionService transactionService, 
            IAccountService accountService,
            INbsClient nbsClient,
            IOptions<BankSettings> appSettings)
        {
            _transactionService = transactionService;
            _accountService = accountService;
            _appSettings = appSettings.Value;
            _nbsClient = nbsClient;
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateTransaction([FromBody] TransactionIDTO transactionIDTO)
        {
            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(transactionIDTO);
                if (transaction == null) return BadRequest();

                var paymentUrl = $"{_appSettings.BankPaymentUrl}".Replace("@TRANSACTION_ID@", transaction.TransactionId.ToString());

                if (transactionIDTO.IsQrCodePayment)
                    paymentUrl = $"{paymentUrl}/qrCode";

                var paymentInstructions = new PaymentInstructionsODTO(paymentUrl)
                {
                    PaymentId = transaction.TransactionId
                };
                return Ok(paymentInstructions);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> PayTransaction([FromBody] PayTransactionIDTO payTransactionIDTO)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(payTransactionIDTO.TransactionId);
            if (transaction == null) return NotFound();

            if (transaction.TransactionLogs!.Any(x => x.TransactionStatus == Enums.TransactionStatus.COMPLETED))
                return BadRequest("Transaction already paid");

            RedirectUrlDTO? redirectUrl = null;
            try
            {
                // TODO: VERIFY CARD
                var isLocalCard = payTransactionIDTO.PanNumber.StartsWith(_appSettings.CardStartNumbers);

                if (!isLocalCard)
                {
                    var isSuccess = await _transactionService.PccSendToPayTransctionAsync(transaction, payTransactionIDTO, _appSettings.PccBankId, _appSettings.PccUrl);
                    if (isSuccess)
                    {
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionSuccessUrl);
                    }
                    else
                    {
                        await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.FAIL);
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                    }
                }
                else
                {
                    var sender = await _accountService.GetAccountByCreditCardAsync(payTransactionIDTO);
                    var isSuccess = await _transactionService.PayTransctionAsync(transaction, sender!);
                    if (isSuccess)
                    {
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionSuccessUrl);
                    }
                    else
                    {
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                    }
                }
            }
            catch (Exception)
            {
                await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.ERROR);
                redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionErrorUrl);
            }

            return Ok(redirectUrl);
        }

        [HttpPost("PCC")]
        public async Task<ActionResult<PccTransactionODTO>> PccPayTransaction([FromBody] PccTransactionIDTO pccTransactionIDTO)
        {
            try
            {
                var transactionODTO = await _transactionService.PccReceiveToPayTransactionAsync(pccTransactionIDTO, _appSettings.CardStartNumbers);
                if (transactionODTO == null) return BadRequest();

                return Ok(transactionODTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("QrCode/{transactionId}")]
        public async Task<ActionResult> GenerateQrCode([FromRoute] int transactionId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);

                if (transaction == null) return NotFound();

                var amount = await _transactionService.ExchangeAsync(transaction.Currency!.Code, "RSD", transaction.Amount);
                if (amount == null) return BadRequest();

                var qrCodeGenIDTO = Converter.ConvertToQrCodoeGenerateIDTO(transaction, (double)amount, "RSD");
                var qrCode = await _nbsClient.GenerateQrCodeAsync(qrCodeGenIDTO);

                if (qrCode == null || qrCode.Status!.Code != 0) return BadRequest();

                return Ok(qrCode.Base64QrCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("QrCode/Input/{transactionId}")]
        public async Task<ActionResult<string>> GetQrCodeInput([FromRoute] int transactionId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);

                if (transaction == null) return NotFound();

                var amount = await _transactionService.ExchangeAsync(transaction.Currency!.Code, "RSD", transaction.Amount);
                if (amount == null) return BadRequest();

                var qrCodeGenIDTO = Converter.ConvertToQrCodoeGenerateIDTO(transaction, (double)amount, "RSD");
                return Ok(qrCodeGenIDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("QrCode/Validate/{transactionId}")]
        public async Task<ActionResult> ValidateQrCode([FromRoute] int transactionId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);

                if (transaction == null) return NotFound();

                var amount = await _transactionService.ExchangeAsync(transaction.Currency!.Code, "RSD", transaction.Amount);
                if (amount == null) return BadRequest();

                var qrCodeGenIDTO = Converter.ConvertToQrCodoeGenerateIDTO(transaction, (double)amount, "RSD");
                var qrCode = await _nbsClient.ValdiateQrCodeAsync(qrCodeGenIDTO);

                if (qrCode == null || qrCode.Status!.Code !=  0 ) return BadRequest();

                return Ok(qrCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("QrCode/Validate")]
        public async Task<ActionResult> ValidateQrCodeString([FromBody] BankvalidateQrCodeIDTO input)
        {
            try
            {
                var qrCode = await _nbsClient.ValdiateQrCodeAsync(input.Input);
                return Ok(qrCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
