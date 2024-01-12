using Base.DTO.Input;
using Base.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using PayPalPaymentService.WebApi.Enums;
using PayPalPaymentService.WebApi.Models;

namespace PayPalPaymentService.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<Invoice?> GetInvoiceByPayPalOrderIdAsync(string payPalOrderId);
        Task<Invoice?> CreateInvoiceAsync(PaymentRequestIDTO paymentRequestDTO, InvoiceType invoiceType, bool recurringPayment);
        Task<Invoice?> GetInvoiceByPayPalSubscriptionIdAsync(string payPalSubscriptionId);
        Task<Invoice?> UpdateInvoiceStatusAsync(int invoiceId, TransactionStatus transactionStatus);
        Task<Invoice?> UpdatePayPalOrderIdAsync(int invoiceId, string payPalOrderId);
        Task<Invoice?> UpdatePayPalSubscriptionIdAsync(int invoiceId, string payPalSubscriptionId);
        Task<Invoice?> UpdatePayerIdAsync(int invoiceId, string payerId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly PayPalServiceContext _context;

        public InvoiceService(PayPalServiceContext context)
        {
            _context = context;
        }

        public async Task<Invoice?> GetInvoiceByPayPalOrderIdAsync(string payPalOrderId)
        {
            return await _context.Invoices
                .Where(x => x.PayPalOrderId == payPalOrderId)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> GetInvoiceByPayPalSubscriptionIdAsync(string payPalSubscriptionId)
        {
            return await _context.Invoices
                .Where(x => x.PayPalSubscriptionId == payPalSubscriptionId)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> CreateInvoiceAsync(PaymentRequestIDTO paymentRequestDTO, InvoiceType invoiceType, bool recurringPayment)
        {
            var merchant = await _context.Merchants
                .Where(x => x.PaymentServiceMerchantId == paymentRequestDTO.MerchantId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (merchant == null) return null;

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
                InvoiceType = invoiceType, 
                RecurringPayment = recurringPayment,
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

        public async Task<Invoice?> UpdatePayPalOrderIdAsync(int invoiceId, string payPalOrderId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.InvoiceLogs)
                .FirstOrDefaultAsync();

            if (invoice == null) return null;

            invoice.PayPalOrderId = payPalOrderId;
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<Invoice?> UpdatePayPalSubscriptionIdAsync(int invoiceId, string payPalSubscriptionId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.InvoiceLogs)
                .FirstOrDefaultAsync();

            if (invoice == null) return null;

            invoice.PayPalSubscriptionId = payPalSubscriptionId;
            await _context.SaveChangesAsync();

            return invoice;
        }

        public async Task<Invoice?> UpdatePayerIdAsync(int invoiceId, string payerId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.InvoiceLogs)
                .FirstOrDefaultAsync();

            if (invoice == null) return null;

            invoice.PayerId = payerId;
            await _context.SaveChangesAsync();

            return invoice;
        }
    }
}
