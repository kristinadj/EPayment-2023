using BankPaymentService.WebApi.Enums;
using BankPaymentService.WebApi.Models;
using Base.DTO.Input;
using Microsoft.EntityFrameworkCore;

namespace BankPaymentService.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<Invoice?> GetInvoiceByInvoiceIdAsync(int invoiceId);
        Task<Invoice?> GetInvoiceByExternalInvoiceIdAsync(int extrenalInvoiceId);
        Task<Invoice?> CreateInvoiceAsync(PaymentRequestIDTO paymentRequestDTO);
        Task<Invoice?> UpdateInvoiceStatusAsync(int invoiceId, TransactionStatus transactionStatus);
        Task<Invoice?> UpdateBankRecurringTransactionId(int invoiceId, int recurringTransactionId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly BankPaymentServiceContext _context;

        public InvoiceService(BankPaymentServiceContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> CreateInvoiceAsync(PaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == paymentRequestDTO.MerchantId)
                .Include(x => x.Bank)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (merchant == null || merchant.Bank == null) return null;

            var currency = await _context.Currencies
                .Where(x => x.Code == paymentRequestDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (currency == null) return null;

            var invoice = new Invoice(paymentRequestDTO.TransactionSuccessUrl, paymentRequestDTO.TransactionFailureUrl, paymentRequestDTO.TransactionErrorUrl)
            {
                ExternalInvoiceId = paymentRequestDTO.ExternalInvoiceId,
                MerchantId = merchant.MerchantId,
                Amount = paymentRequestDTO.Amount,
                CurrencyId = currency.CurrencyId,
                Timestamp = paymentRequestDTO.Timestamp,
                TransactionStatus = TransactionStatus.CREATED,
                InvoiceLogs = new List<InvoiceLog>
                {
                     new InvoiceLog { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<Invoice?> GetInvoiceByExternalInvoiceIdAsync(int extrenalInvoiceId)
        {
            return await _context.Invoices
                .Where(x => x.ExternalInvoiceId == extrenalInvoiceId)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> UpdateBankRecurringTransactionId(int invoiceId, int recurringTransactionId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.InvoiceLogs)
                .FirstOrDefaultAsync();

            if (invoice == null) return null;

            invoice.BankRecurringTransactionId = recurringTransactionId;
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<Invoice?> GetInvoiceByInvoiceIdAsync(int invoiceId)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> UpdateInvoiceStatusAsync(int invoiceId, TransactionStatus transactionStatus)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.InvoiceLogs)
                .FirstOrDefaultAsync();

            if (invoice == null) return null;

            invoice.TransactionStatus = transactionStatus;
            invoice.InvoiceLogs!.Add(new InvoiceLog { TransactionStatus = transactionStatus, Timestamp = DateTime.Now });
            await _context.SaveChangesAsync();

            return invoice;
        }
    }
}
