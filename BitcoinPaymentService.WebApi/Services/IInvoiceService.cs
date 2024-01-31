using Base.DTO.Input;
using BitcoinPaymentService.WebApi.Enums;
using BitcoinPaymentService.WebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcoinPaymentService.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<Invoice?> GetInvoiceByIdAsync(int id);
        Task<Invoice?> GetInvoiceByExternalPaymentServiceInvoiceIdAsync(string externalInvoiceId);
        Task<Invoice?> CreateInvoiceAsync(PaymentRequestIDTO paymentRequestDTO);
        Task<Invoice?> UpdateInvoiceStatusAsync(int invoiceId, TransactionStatus transactionStatus);
        Task<Invoice?> UpdateExternalPaymentServiceInvoiceIdAsync(int invoiceId, string externalInvoiceId);
        Task<bool> IsInvoicePaidAsync(int externalInvoiceId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly BitcoinServiceContext _context;

        public InvoiceService(BitcoinServiceContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.Merchant)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> GetInvoiceByExternalPaymentServiceInvoiceIdAsync(string externalInvoiceId)
        {
            return await _context.Invoices
                .Where(x => x.ExternalPaymentServiceInvoiceId == externalInvoiceId)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> CreateInvoiceAsync(PaymentRequestIDTO paymentRequestDTO)
        {
            var merchant = await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == paymentRequestDTO.MerchantId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (merchant == null) return null;

            var currency = await _context.Currencies
                .Where(x => x.Code == paymentRequestDTO.CurrencyCode)
                .FirstOrDefaultAsync();

            if (currency == null) return null;

            var invoice = new Invoice(paymentRequestDTO.TransactionSuccessUrl, paymentRequestDTO.TransactionFailureUrl, paymentRequestDTO.TransactionErrorUrl)
            {
                ExternalInvoiceId = paymentRequestDTO.ExternalInvoiceId,
                MerchantId = merchant.MerchantId,
                Amount = paymentRequestDTO.Amount,
                Currency = currency,
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

        public async Task<Invoice?> UpdateExternalPaymentServiceInvoiceIdAsync(int invoiceId, string externalInvoiceId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.InvoiceLogs)
                .FirstOrDefaultAsync();

            if (invoice == null) return null;

            invoice.ExternalPaymentServiceInvoiceId = externalInvoiceId;
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<bool> IsInvoicePaidAsync(int externalInvoiceId)
        {
            return await _context.Invoices
                .Where(x => x.ExternalInvoiceId == externalInvoiceId && x.TransactionStatus == TransactionStatus.COMPLETED)
                .AnyAsync();
        }
    }
}
