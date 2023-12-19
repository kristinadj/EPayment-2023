using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Enums;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId);
        Task<InvoiceODTO?> CreateInvoiceAsync(int orderId, int paymentMethodId);
        Task<InvoiceODTO?> CreateInvoiceForSubscriptionPlanAsync(UserSubscriptionPlan userSubscripptionPlan);
        Task UpdateInvoiceTransactionStatusasync(int invoiceId, TransactionStatus transactionStatus);
        Task<bool> UpdatePaymentMethodAsync(int invoiceId, int pspPaymentMethodId);
    }

    public class InvoiceService : IInvoiceService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public InvoiceService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<InvoiceODTO?> CreateInvoiceAsync(int orderId, int paymentMethodId)
        {
            var order = await _context.Orders
                .Where(x => x.OrderId == orderId && x.OrderStatus == OrderStatus.CREATED)
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync();

            var paymentMethod = await _context.PaymentMethods
                .Where(x => x.PaymentMethodId == paymentMethodId)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (order == null || paymentMethod == null) return null;

            var invoice = new Invoice(string.Empty) // TODO: Fix
            {
                InvoiceType = InvoiceType.ORDER,
                MerchantId = order.MerchantId,
                TotalPrice = order.OrderItems!.Select(x => x.Quantity * x.Price).Sum(),
                CurrencyId = order.OrderItems!.Select(x => x.CurrencyId).FirstOrDefault(),
                Transaction = new Transaction
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId,
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = TransactionStatus.CREATED,
                    TransactionLogs = new List<TransactionLog>
                    {
                        new TransactionLog
                        {
                            TransactionStatus = TransactionStatus.CREATED,
                            Timestamp = DateTime.Now,
                        }
                    }
                },
            };
            order.Invoice = invoice;

            //await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return await _context.Invoices
                .Where(x => x.InvoiceId == invoice.InvoiceId)
                .Include(x => x!.Merchant)
                .Include(x => x.Currency)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<InvoiceODTO?> CreateInvoiceForSubscriptionPlanAsync(UserSubscriptionPlan userSubscripptionPlan)
        {
            var subscriptionPlan = await _context.SubscriptionPlans
                .Where(x => x.SubscriptionPlanId == userSubscripptionPlan.SubscriptionPlanId)
                .FirstOrDefaultAsync();

            if (subscriptionPlan == null) return null;

            var merchant = await _context.Merchants
                .Where(x => x.IsMasterMerchant)
                .FirstOrDefaultAsync();

            var invoice = new Invoice(userSubscripptionPlan.UserId)
            {
                InvoiceType = InvoiceType.SUBSCRIPTION_PLAN,
                MerchantId = merchant!.MerchantId,
                TotalPrice = subscriptionPlan.Price,
                CurrencyId = subscriptionPlan.CurrencyId,
                Transaction = new Transaction
                {
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = TransactionStatus.CREATED,
                    TransactionLogs = new List<TransactionLog>
                    {
                        new() {
                            TransactionStatus = TransactionStatus.CREATED,
                            Timestamp = DateTime.Now,
                        }
                    }
                },
            };
            userSubscripptionPlan.Invoice = invoice;
            _context.Entry(userSubscripptionPlan).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return await _context.Invoices
                .Where(x => x.InvoiceId == invoice.InvoiceId)
                .Include(x => x.Merchant)
                .Include(x => x.Currency)
                .Include(x => x.Transaction)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateInvoiceTransactionStatusasync(int invoiceId, TransactionStatus transactionStatus)
        {
            var transaction = await _context.Transactions
                .Where(x => x.Invoice!.InvoiceId == invoiceId)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();

            if (transaction == null) return;

            transaction.TransactionStatus = transactionStatus;
            transaction.TransactionLogs!.Add(new TransactionLog
            {
                TransactionId = transaction.TransactionId,
                TransactionStatus = transactionStatus,
                Timestamp = DateTime.Now,
            });

            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdatePaymentMethodAsync(int invoiceId, int pspPaymentMethodId)
        {
            var invoice = await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId && x.Transaction != null)
                .Include(x => x.Transaction)
                .FirstOrDefaultAsync();

            var paymentMethod = await _context.PaymentMethods
                .Where(x => x.PspPaymentMethodId == pspPaymentMethodId)
                .FirstOrDefaultAsync();

            if (invoice == null || paymentMethod == null) return false;

            invoice.Transaction!.PaymentMethodId = paymentMethod.PaymentMethodId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
