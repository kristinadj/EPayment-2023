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

        public TransactionService(WebShopContext context)
        {
            _context = context;
        }

        public async Task<bool> UpdateTransactionStatusAsync(int transactionId, TransactionStatus transactionStatus)
        {
            var transaction = await _context.Transactions
                .Where(x => x.TransactionId == transactionId)
                .Include(x => x.Invoice)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();

            if (transaction == null) throw new Exception($"Transaction {transactionId} not found");

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
                    .Include(x => x.MerchantOrders)!
                    .ThenInclude(x => x.Invoice)
                    .ThenInclude(x => x!.Transaction)
                    .FirstOrDefaultAsync();

                if (order == null) throw new Exception($"Order for invoice {transaction.Invoice!.InvoiceId} not found");

                if (order.OrderStatus == OrderStatus.CREATED || order.OrderStatus != OrderStatus.PARTIALLY_COMPLETED)
                {
                    if (transactionStatus == TransactionStatus.COMPLETED)
                    {
                        var isCompleted = order.MerchantOrders!.All(x => x.Invoice != null && x.Invoice.Transaction!.TransactionStatus == TransactionStatus.COMPLETED);

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
