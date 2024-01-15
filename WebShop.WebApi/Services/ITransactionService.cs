using AutoMapper;
using Base.DTO.Enums;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Enums;
using WebShop.WebApi.Models;

namespace WebShop.WebApi.Services
{
    public interface ITransactionService
    {
        Task<bool> UpdateTransactionStatusAsync(int transactionId, TransactionStatus transactionStatus);
    }

    public class TransactionService : ITransactionService
    {
        private readonly WebShopContext _context;
        private readonly IMapper _mapper;

        public TransactionService(WebShopContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> UpdateTransactionStatusAsync(int transactionId, TransactionStatus transactionStatus)
        {
            var transaction = await _context.Transactions
                .Where(x => x.TransactionId == transactionId)
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

            if (transaction.Invoice!.InvoiceType == InvoiceType.ORDER)
            {
                var order = await _context.Orders
                    .Where(x => x.MerchantOrders!.Any(x => x.InvoiceId == transaction.Invoice!.InvoiceId))
                    .Include(x => x.OrderLogs)
                    .Include(x => x.MerchantOrders)
                    .FirstOrDefaultAsync();

                if (order == null) return false;

                if (order.OrderStatus == OrderStatus.CREATED || order.OrderStatus != OrderStatus.PARTIALLY_COMPLETED)
                {
                    if (transactionStatus == TransactionStatus.COMPLETED)
                    {
                        var isCompleted = order.MerchantOrders!.All(x => x.Invoice!.Transaction!.TransactionStatus == TransactionStatus.COMPLETED);

                        if (isCompleted)
                        {
                            order.OrderStatus = OrderStatus.COMPLETED;
                            order.OrderLogs!.Add(new OrderLog
                            {
                                OrderStatus = OrderStatus.COMPLETED,
                                Timestamp = DateTime.Now
                            });
                        }
                        else
                        {
                            order.OrderStatus = OrderStatus.PARTIALLY_COMPLETED;
                            order.OrderLogs!.Add(new OrderLog
                            {
                                OrderStatus = OrderStatus.PARTIALLY_COMPLETED,
                                Timestamp = DateTime.Now
                            });
                        }
                    }
                    else if (transactionStatus == TransactionStatus.FAIL || transactionStatus == TransactionStatus.ERROR)
                    {
                        order.OrderStatus = OrderStatus.INVALID;
                        order.OrderLogs!.Add(new OrderLog
                        {
                            OrderStatus = OrderStatus.INVALID,
                            Timestamp = DateTime.Now
                        });
                    }
                }
            }
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
