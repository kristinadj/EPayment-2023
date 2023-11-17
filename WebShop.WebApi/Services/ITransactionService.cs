using AutoMapper;
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
                .ThenInclude(x => x!.Order)
                .ThenInclude(x => x!.OrderLogs)
                .Include(x => x.TransactionLogs)
                .FirstOrDefaultAsync();

            if (transaction == null) return false;

            transaction.TransactionStatus = transactionStatus;
            transaction.TransactionLogs!.Add(new TransactionLog
            {
                TransactionStatus = transactionStatus,
                Timestamp = DateTime.Now
            });

            if (transactionStatus == TransactionStatus.COMPLETED)
            {
                transaction.Invoice!.Order!.OrderStatus = OrderStatus.COMPLETED;
                transaction.Invoice!.Order!.OrderLogs!.Add(new OrderLog
                {
                    OrderStatus = OrderStatus.COMPLETED,
                    Timestamp = DateTime.Now
                });
            }
            else if (transactionStatus == TransactionStatus.FAIL || transactionStatus == TransactionStatus.ERROR)
            {
                transaction.Invoice!.Order!.OrderStatus = OrderStatus.INVALID;
                transaction.Invoice!.Order!.OrderLogs!.Add(new OrderLog
                {
                    OrderStatus = OrderStatus.INVALID,
                    Timestamp = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
