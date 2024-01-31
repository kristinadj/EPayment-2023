using Bank.DTO.Input;
using Bank2.WebApi.AppSettings;
using Bank2.WebApi.Helpers;
using Bank2.WebApi.Models;
using Bank2.WebApi.Services;
using Base.DTO.Input;
using Base.DTO.NBS;
using Base.DTO.Output;
using Base.DTO.Shared;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Bank2.WebApi.Controllers
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

        [HttpGet("Status/{transactionId}")]
        public async Task<ActionResult<RedirectUrlDTO?>> GeTransactionStatus([FromRoute] int transactionId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);

                if (transaction!.TransactionStatus == Enums.TransactionStatus.CREATED || transaction!.TransactionStatus == Enums.TransactionStatus.IN_PROGRESS)
                    return BadRequest("Transaction is still in progress");

                RedirectUrlDTO? redirectUrl;
                if (transaction.TransactionStatus == Enums.TransactionStatus.COMPLETED)
                {
                    redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionSuccessUrl);
                }
                else if (transaction.TransactionStatus == Enums.TransactionStatus.FAIL)
                {
                    redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                }
                else
                {
                    redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionErrorUrl);
                }

                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PaymentInstructionsODTO>> CreateTransaction([FromBody] TransactionIDTO transactionIDTO)
        {
            try
            {
                var transaction = await _transactionService.CreateTransactionAsync(transactionIDTO);
                if (transaction == null) return BadRequest("One of the parameters is invalid: CurrencyCode / SenderId / AccountNumber");

                var paymentUrl = $"{_appSettings.BankPaymentUrl}".Replace("@TRANSACTION_ID@", transaction.TransactionId.ToString());

                if (transactionIDTO.IsQrCodePayment)
                    paymentUrl = $"{paymentUrl}/qrCode";

                var paymentInstructions = new PaymentInstructionsODTO(transaction.TransactionId.ToString(), paymentUrl);
                return Ok(paymentInstructions);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<RedirectUrlDTO?>> PayTransaction([FromBody] PayTransactionIDTO payTransactionIDTO)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(payTransactionIDTO.TransactionId);

            if (transaction!.TransactionLogs!.Any(x => x.TransactionStatus == Enums.TransactionStatus.COMPLETED))
                return Conflict("Transaction is already paid");

            var recurringTransactionDefinition = await _transactionService.GetReccurringTransactionDefinitionByTransactionIdAsync(payTransactionIDTO.TransactionId);
            var successUrl = transaction.TransactionSuccessUrl;
            if (recurringTransactionDefinition != null)
            {
                successUrl = $"{transaction.TransactionSuccessUrl}/{recurringTransactionDefinition.RecurringTransactionDefinitionId}";
                await _transactionService.UpdatePaymentDataAsync(recurringTransactionDefinition, payTransactionIDTO);
            }

            RedirectUrlDTO? redirectUrl = null;
            try
            {
                var isExpired = CardChecker.IsCardExpired(payTransactionIDTO.ExpiratoryDate);
                if (isExpired)
                {
                    return BadRequest("Card is expired");
                }

                var isLocalCard = payTransactionIDTO.PanNumber.StartsWith(_appSettings.CardStartNumbers);

                if (!isLocalCard)
                {
                    var isSuccess = await _transactionService.PccSendAcquirerTransactionAsync(transaction, payTransactionIDTO, _appSettings.PccBankId, _appSettings.PccUrl);
                    if (isSuccess)
                    {
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(successUrl);
                    }
                    else
                    {
                        await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.FAIL);
                        redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                    }
                }
                else
                {
                    var sender = await _accountService.GetAccountByCreditCardAsync(payTransactionIDTO.CardHolderName, payTransactionIDTO.PanNumber, payTransactionIDTO.ExpiratoryDate, payTransactionIDTO.CVV);

                    var isSuccess = false;
                    if (sender != null)
                    {
                        isSuccess = await _transactionService.PayLocalTransactionAsync(transaction, sender!);
                        if (isSuccess)
                        {
                            redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(successUrl);
                        }
                        else
                        {
                            return BadRequest("Insufficient balance");
                        }
                    }
                    else
                    {
                        return BadRequest("Invalid card information");
                    }

                    if (!isSuccess)
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

        [HttpPut("Failed/{transactionId}")]
        public async Task<ActionResult> UpdateTransactionFailed([FromRoute] int transactionId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);
                if (transaction == null) return NotFound($"Transaction {transactionId} not found");

                await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.FAIL);

                var redirectUrl = await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                return Ok(redirectUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PCC/ReceiveAquirerTransaction")]
        public async Task<ActionResult<PccAquirerTransactionODTO>> PccPayAcquirerTransaction([FromBody] PccAquirerTransactionIDTO pccTransactionIDTO)
        {
            try
            {
                var transactionODTO = await _transactionService.PccReceiveAquirerTransactionAsync(pccTransactionIDTO, _appSettings.CardStartNumbers);
                return Ok(transactionODTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("PCC/ReceiveIssuerTransaction")]
        public async Task<ActionResult<PccAquirerTransactionODTO>> PccPayIssuerTransaction([FromBody] PccIssuerTransactionIDTO pccTransactionIDTO)
        {
            try
            {
                var transactionODTO = await _transactionService.PccReceiveIsssuerTransactionAsync(pccTransactionIDTO);
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
                var amount = await _transactionService.ExchangeAsync(transaction!.Currency!.Code, "RSD", transaction.Amount);
                var qrCodeGenIDTO = Converter.ConvertToQrCodeGenerateIDTO(transaction, (double)amount, "RSD");
                var qrCode = await _nbsClient.GenerateQrCodeAsync(qrCodeGenIDTO);

                if (qrCode == null || qrCode.Status!.Code != 0) return BadRequest("Unexpected error while generating QR code");

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
                var amount = await _transactionService.ExchangeAsync(transaction!.Currency!.Code, "RSD", transaction.Amount);
                var qrCodeGenIDTO = Converter.ConvertToQrCodeGenerateIDTO(transaction, (double)amount, "RSD");
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
                var amount = await _transactionService.ExchangeAsync(transaction!.Currency!.Code, "RSD", transaction.Amount);

                var qrCodeGenIDTO = Converter.ConvertToQrCodeGenerateIDTO(transaction, (double)amount, "RSD");
                var qrCode = await _nbsClient.ValdiateQrCodeAsync(qrCodeGenIDTO);

                if (qrCode == null || qrCode.Status!.Code != 0) return BadRequest("Unexpected error while generating QR code");

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

        [HttpPost("QrCode/Pay")]
        public async Task<ActionResult> PayQrCodeTransaction([FromBody] QrCodePaymentIDTO qrCodePayment)
        {
            Transaction? transaction = null;

            try
            {
                var senderAccount = await _accountService.GetAccountByCustomerIdAsync(qrCodePayment.PayerId);
                if (senderAccount == null) return NotFound("Invalid payer account");

                var qrCode = await _nbsClient.ValdiateQrCodeAsync(qrCodePayment.ScannedQrCode);
                if (qrCode == null || qrCode.Status!.Code != 0) return BadRequest("NBS - Invalid QR code format");

                var acquirerAccount = await _accountService.GetAcountByAccountNumberAsync(qrCode.Data!.R, false);

                if (acquirerAccount == null)
                {
                    var isSuccess = await _transactionService.PccSendIssuerTransactionAsync(transaction!, senderAccount, _appSettings.PccBankId, _appSettings.PccUrl);
                    if (isSuccess)
                    {
                        await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionSuccessUrl);
                    }
                    else
                    {
                        await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.FAIL);
                        await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                    }
                }
                else
                {
                    var isSuccess = int.TryParse(qrCode.Data!.S, out int extrenalTransactionId);
                    if (!isSuccess) return BadRequest("TransactionId is not encoded in QR code");

                    transaction = await _transactionService.GetTransactionByBankPaymentServiceTransactionIdIdAsync(extrenalTransactionId);
                    if (transaction == null) return NotFound($"Transaction {extrenalTransactionId} not found");

                    if (transaction!.TransactionStatus == Enums.TransactionStatus.COMPLETED) return BadRequest("Transaction is already settled");

                    await _transactionService.UpdateAcquirerAccountIdAsync(transaction, acquirerAccount);
                    isSuccess = await _transactionService.PayLocalTransactionAsync(transaction, senderAccount!);
                    if (isSuccess)
                    {
                        await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionSuccessUrl);
                    }
                    else
                    {
                        await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.FAIL);
                        await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionFailureUrl);
                        return BadRequest("Insufficient balance");
                    }
                }

                return Ok();
            }
            catch (Exception ex)
            {
                if (transaction != null)
                {
                    await _transactionService.UpdateTransactionStatusAsync(transaction, Enums.TransactionStatus.ERROR);
                    await _transactionService.UpdatePaymentServiceInvoiceStatusAsync(transaction.TransactionErrorUrl);
                }

                return BadRequest(ex.Message);
            }
        }
    }
}
