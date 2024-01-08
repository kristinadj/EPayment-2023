using AutoMapper;
using AutoMapper.QueryableExtensions;
using Base.DTO.Enums;
using Base.DTO.Shared;
using Microsoft.EntityFrameworkCore;
using PSP.WebApi.DTO.Output;
using PSP.WebApi.Enums;
using PSP.WebApi.Models;

namespace PSP.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceODTO?> GetInvoiceByIdAsyync(int invoiceId);
        Task<SubscriptionDetails?> GetSubscriptioNdetailsByInvoiceIdAsync(int invoiceId);
        Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PspInvoiceIDTO invoiceIDTO, InvoiceType invoiceType);
        Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PspSubscriptionPaymentDTO subscriptionPaymentIDTO);
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

        public async Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PspInvoiceIDTO invoiceIDTO, InvoiceType invoiceType)
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
                InvoiceType = invoiceType,
                RecurringPayment = false,
                Transaction = new Transaction
                {
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = TransactionStatus.CREATED,
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

        public async Task<Invoice?> CreateInvoiceAsync(Merchant merchant, PspSubscriptionPaymentDTO subscriptionPaymentIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == subscriptionPaymentIDTO.CurrencyCode)
                .FirstOrDefaultAsync();

            if (currency == null) return null;

            var invoice = new Invoice(subscriptionPaymentIDTO.IssuedToUserId)
            {
                ExternalInvoiceId = subscriptionPaymentIDTO.ExternalInvoiceId,
                MerchantId = merchant.MerchantId,
                TotalPrice = subscriptionPaymentIDTO.TotalPrice,
                Currency = currency,
                InvoiceType = InvoiceType.SUBSCRIPTION,
                RecurringPayment = true,
                Transaction = new Transaction
                {
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = TransactionStatus.CREATED,
                    TransactionLogs = new List<TransactionLog>()
                    {
                        new TransactionLog { TransactionStatus = TransactionStatus.CREATED, Timestamp = DateTime.Now }
                    }
                }
            };

            var subscriptionDetails = new SubscriptionDetails
            {
                SubscriberName = subscriptionPaymentIDTO.Subscriber!.Name,
                SubscriberEmail = subscriptionPaymentIDTO.Subscriber.Email,
                BrandName = subscriptionPaymentIDTO.BrandName,
                ProductName = subscriptionPaymentIDTO.Product!.Name,
                ProductCategory = subscriptionPaymentIDTO.Product.Category,
                ProductType = subscriptionPaymentIDTO.Product.Type,
                ProductDescription = subscriptionPaymentIDTO.Product.Description,
                Invoice = invoice
            };

            await _context.SubscriptionDetails.AddAsync(subscriptionDetails);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<SubscriptionDetails?> GetSubscriptioNdetailsByInvoiceIdAsync(int invoiceId)
        {
            return await _context.SubscriptionDetails
                .Where(x => x.InvoiceId == invoiceId)
                .FirstOrDefaultAsync();
        }
    }
}
