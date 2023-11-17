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
        Task<InvoiceODTO?> CreateInvoiceAsync(int orderId, int paymentMethodId);
        Task UpdateInvoiceTransactionStatusasync(int invoiceId, TransactionStatus transactionStatus);
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

            var invoice = new Invoice
            {
                OrderId = orderId,
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

            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();

            return await _context.Invoices
                .Where(x => x.InvoiceId == invoice.InvoiceId)
                .Include(x => x.Order)
                .ThenInclude(x => x!.Merchant)
                .Include(x => x.Order)
                .Include(x => x.Currency)
                .ProjectTo<InvoiceODTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateInvoiceTransactionStatusasync(int invoiceId, TransactionStatus transactionStatus)
        {
            var transaction = await _context.Transactions
                .Where(x => x.InvoiceId == invoiceId)
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
    }
}
