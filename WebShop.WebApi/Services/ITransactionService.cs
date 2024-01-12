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
                .Where(x => x.InvoiceId == transaction.Invoice!.InvoiceId)
                .Include(x => x.OrderLogs)
                .FirstOrDefaultAsync();

                if (order == null) return false;

                if (transactionStatus == TransactionStatus.COMPLETED)
                {
                    order.OrderStatus = OrderStatus.COMPLETED;
                    order.OrderLogs!.Add(new OrderLog
                    {
                        OrderStatus = OrderStatus.COMPLETED,
                        Timestamp = DateTime.Now
                    });
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
            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
