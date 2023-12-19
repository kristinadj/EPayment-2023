using AutoMapper;
using AutoMapper.QueryableExtensions;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Enums;
using PSP.WebApi.Models;
using System.Threading.Tasks;

namespace PSP.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceODTO? > GetInvoiceByIdAsyync(int invoiceId);
        Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PaymentMethod paymentMethod, PspInvoiceIDTO invoiceIDTO);
        Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PspInvoiceIDTO invoiceIDTO);
        Task<bool> UpdateTransactionStatusAsync(int invoiceId, TransactionStatus transactionStatus);
        Task<Invoice?> UpdatePaymentMethodAsync(int invoiceId, int paymentMethodId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly PspContext _context;
        private readonly IMapper _mapper;

        public InvoiceService(PspContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsyync(int invoiceId)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .ProjectTo<InvoiceODTO?>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PaymentMethod paymentMethod, PspInvoiceIDTO invoiceIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == invoiceIDTO.CurrencyCode)
                .FirstOrDefaultAsync();

            if (currency == null) return null;

            var invoice = new Invoice(invoiceIDTO.IssuedToUserId)
            {
                ExternalInvoiceId = invoiceIDTO.ExternalInvoiceId,
                MerchantId = merchant.MerchantId,
                TotalPrice = invoiceIDTO.TotalPrice,
                Currency = currency,
                Transaction = new Transaction
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = Enums.TransactionStatus.CREATED,
                    TransactionLogs = new List<TransactionLog>()
                    {
                        new TransactionLog { TransactionStatus = Enums.TransactionStatus.CREATED, Timestamp = DateTime.Now }
                    }
                }
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PspInvoiceIDTO invoiceIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == invoiceIDTO.CurrencyCode)
                .FirstOrDefaultAsync();

            if (currency == null) return null;

            var invoice = new Invoice(invoiceIDTO.IssuedToUserId)
            {
                ExternalInvoiceId = invoiceIDTO.ExternalInvoiceId,
                MerchantId = merchant.MerchantId,
                TotalPrice = invoiceIDTO.TotalPrice,
                Currency = currency,
                Transaction = new Transaction
                {
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = Enums.TransactionStatus.CREATED,
                    TransactionLogs = new List<TransactionLog>()
                    {
                        new TransactionLog { TransactionStatus = Enums.TransactionStatus.CREATED, Timestamp = DateTime.Now }
                    }
                }
            };

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> UpdateTransactionStatusAsync(int invoiceId, TransactionStatus transactionStatus)
        {
            var transaction = await _context.Transactions
                .Where(x => x.Invoice!.InvoiceId == invoiceId)
                .Include(x => x.Invoice)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();

            if (transaction == null) return false;

            transaction.TransactionStatus = transactionStatus;
            transaction.TransactionLogs!.Add(new TransactionLog
            {
                TransactionStatus = transactionStatus,
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Invoice?> UpdatePaymentMethodAsync(int invoiceId, int paymentMethodId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Include(x => x.Transaction)
                .Include(x => x.Merchant)
                .ThenInclude(x => x.PaymentMethods)
                .Include(x => x.Currency)
                .FirstOrDefaultAsync();

            var paymentMethod = await _context.PaymentMethods
                .Where(x => x.PaymentMethodId == paymentMethodId)
                .FirstOrDefaultAsync();

            if (invoice == null || paymentMethod == null) return null;

            invoice.Transaction!.PaymentMethodId = paymentMethod.PaymentMethodId;
            await _context.SaveChangesAsync();

            return invoice;
        }
    }
}
