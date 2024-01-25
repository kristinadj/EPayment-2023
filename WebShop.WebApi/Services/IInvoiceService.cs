using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Enums;
using WebShop.DTO.Output;
using WebShop.WebApi.Models;
using InvoiceType = Base.DTO.Enums.InvoiceType;
using TransactionLog = WebShop.WebApi.Models.TransactionLog;

namespace WebShop.WebApi.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId);
        Task<InvoiceODTO?> CreateInvoiceAsync(int orderId);
        Task<Invoice?> CreateInvoiceForSubscriptionPlanAsync(UserSubscriptionPlan userSubscripptionPlan);
        Task UpdateInvoiceTransactionStatusasync(int invoiceId, TransactionStatus transactionStatus);
        Task<bool> UpdatePaymentMethodAsync(int invoiceId, int pspPaymentMethodId);
        Task<PaymentMethod?> GetPaymentMethodByInvoiceIdAsync(int invoiceId);
        Task<List<InvoiceODTO>> GetInvoicesByBuyerIdAsync(string userId);
        Task<List<InvoiceODTO>> GetInvoicesByMerchantIdAsync(string userId);
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

        public async Task<InvoiceODTO?> CreateInvoiceAsync(int merchantOrderId)
        {
            var merchantOrder = await _context.MerchantOrders
                .Where(x => x.MerchantOrderId == merchantOrderId && (x.Order!.OrderStatus == OrderStatus.CREATED || x.Order.OrderStatus == OrderStatus.PARTIALLY_COMPLETED))
                .Include(x => x.Order)
                .Include(x => x.OrderItems)
                .FirstOrDefaultAsync();

            if (merchantOrder == null) throw new Exception($"MerchantOrder {merchantOrderId} not found");

            var invoice = new Invoice(merchantOrder.Order!.UserId)
            {
                InvoiceType = InvoiceType.ORDER,
                MerchantId = merchantOrder.MerchantId,
                TotalPrice = merchantOrder.OrderItems!.Select(x => x.Quantity * x.Price).Sum(),
                CurrencyId = merchantOrder.OrderItems!.Select(x => x.CurrencyId).FirstOrDefault(),
                Transaction = new Transaction
                {
                    CreatedTimestamp = DateTime.Now,
                    TransactionStatus = TransactionStatus.CREATED,
                    TransactionLogs = new List<TransactionLog>
                    {
                        new()
                        {
                            TransactionStatus = TransactionStatus.CREATED,
                            Timestamp = DateTime.Now,
                        }
                    }
                },
            };

            merchantOrder.Invoice = invoice;
            await _context.SaveChangesAsync();

            return await _context.Invoices
                .Where(x => x.InvoiceId == invoice.InvoiceId)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<Invoice?> CreateInvoiceForSubscriptionPlanAsync(UserSubscriptionPlan userSubscripptionPlan)
        {
            var subscriptionPlan = await _context.SubscriptionPlans
                .Where(x => x.SubscriptionPlanId == userSubscripptionPlan.SubscriptionPlanId)
            .FirstOrDefaultAsync();

            if (subscriptionPlan == null) throw new Exception($"SubscriptionPlan {userSubscripptionPlan.SubscriptionPlanId} not found");

            var merchant = await _context.Merchants
                .Where(x => x.IsMasterMerchant)
                .FirstOrDefaultAsync();

            var invoice = new Invoice(userSubscripptionPlan.UserId)
            {
                InvoiceType = InvoiceType.SUBSCRIPTION,
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
                .ThenInclude(x => x!.User)
                .Include(x => x.Currency)
                .Include(x => x.Transaction)
                .Include(x => x.User)
                .FirstOrDefaultAsync();
        }

        public async Task<InvoiceODTO?> GetInvoiceByIdAsync(int invoiceId)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<List<InvoiceODTO>> GetInvoicesByBuyerIdAsync(string userId)
        {
            var invoices = await _context.Invoices
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Transaction!.CreatedTimestamp)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var invoice in invoices)
            {
                if (invoice.InvoiceType == InvoiceType.ORDER)
                {
                    invoice.MerchantOrder = await _context.MerchantOrders
                        .Where(x => x.InvoiceId == invoice.InvoiceId)
                        .ProjectTo<MerchantOrderODTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                }
                else if (invoice.InvoiceType == InvoiceType.SUBSCRIPTION)
                {
                    invoice.UserSubscriptionPlan = await _context.UserSubscriptionPlans
                        .Where(x => x.InvoiceId == invoice.InvoiceId)
                        .ProjectTo<UserSubscriptionPlanODTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                }
            }

            return invoices;
        }

        public async Task<List<InvoiceODTO>> GetInvoicesByMerchantIdAsync(string userId)
        {
            var invoices = await _context.Invoices
                .Where(x => x.Merchant!.UserId == userId)
                .OrderByDescending(x => x.Transaction!.CreatedTimestamp)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            foreach (var invoice in invoices)
            {
                if (invoice.InvoiceType == InvoiceType.ORDER)
                {
                    invoice.MerchantOrder = await _context.MerchantOrders
                        .Where(x => x.InvoiceId == invoice.InvoiceId)
                        .ProjectTo<MerchantOrderODTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                }
                else if (invoice.InvoiceType == InvoiceType.SUBSCRIPTION)
                {
                    invoice.UserSubscriptionPlan = await _context.UserSubscriptionPlans
                        .Where(x => x.InvoiceId == invoice.InvoiceId)
                        .ProjectTo<UserSubscriptionPlanODTO>(_mapper.ConfigurationProvider)
                        .FirstOrDefaultAsync();
                }
            }

            return invoices;
        }

        public async Task<PaymentMethod?> GetPaymentMethodByInvoiceIdAsync(int invoiceId)
        {
            return await _context.Invoices
                .Where(x => x.InvoiceId == invoiceId)
                .Select(x => x.Transaction!.PaymentMethod)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateInvoiceTransactionStatusasync(int invoiceId, TransactionStatus transactionStatus)
        {
            var transaction = await _context.Transactions
                .Where(x => x.Invoice!.InvoiceId == invoiceId)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();

            if (transaction == null) throw new Exception($"Transaction for invocie {invoiceId} not found");

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
