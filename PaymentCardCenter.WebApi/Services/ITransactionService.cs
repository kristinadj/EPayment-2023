using Base.DTO.Input;
using Base.DTO.Output;
using Microsoft.EntityFrameworkCore;
using PaymentCardCenter.WebApi.Models;

namespace PaymentCardCenter.WebApi.Services
{
    public interface ITransactionService
    {
        Task<PccTransactionODTO?> ResolvePaymentAsync(PccTransactionIDTO transactionIDTO);
    }

    public class TransactionService : ITransactionService
    {
        private readonly PccContext _context;

        public TransactionService(PccContext pccContext)
        {
            _context = pccContext;
        }

        public async Task<PccTransactionODTO?> ResolvePaymentAsync(PccTransactionIDTO transactionIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) return null;

            var aquirerBank = await _context.Banks
                .Where(x => x.BankId == transactionIDTO.AquirerBankId)
                .FirstOrDefaultAsync();

            if (aquirerBank == null) return null;

            var transaction = new Transaction
            {
                AquirerBankId = aquirerBank.BankId,
                AquirerTransctionId = transactionIDTO.AquirerTransctionId,
                AquirerTimestamp = transactionIDTO.AquirerTimestamp,
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                TransactionStatus = Enums.TransactionStatus.CREATED,
                TransactionLogs = new List<TransactionLog>
                {
                    new TransactionLog { TransactionStatus = Enums.TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var issuerBank = await _context.Banks
                .Where(x => transactionIDTO.PayTransaction!.PanNumber.StartsWith(x.CardStartNumbers))
                .FirstOrDefaultAsync();

            if (issuerBank == null)
            {
                transaction.TransactionStatus = Enums.TransactionStatus.ERROR;
                transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.ERROR, Timestamp = DateTime.Now });
                await _context.SaveChangesAsync();
                return null;
            }

            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsJsonAsync($"{issuerBank.RedirectUrl}/Transaction/PCC", transactionIDTO);
                if (response.IsSuccessStatusCode)
                {
                    var transactionODTO = await response.Content.ReadFromJsonAsync<PccTransactionODTO?>();
                    if (transactionODTO != null && transactionODTO.IsSuccess)
                    {
                        transaction.IssuerBankId = issuerBank.BankId;
                        transaction.IssuerTransactionId = transactionODTO!.IssuerTransactionId;
                        transaction.IssuerTimestamp = transactionODTO!.IssuerTimestamp;
                        transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
                        transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.COMPLETED, Timestamp = DateTime.Now });
                    }
                    else
                    {
                        transaction.IssuerBankId = issuerBank.BankId;
                        transaction.IssuerTransactionId = transactionODTO!.IssuerTransactionId;
                        transaction.IssuerTimestamp = transactionODTO!.IssuerTimestamp;
                        transaction.TransactionStatus = Enums.TransactionStatus.FAIL;
                        transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.FAIL, Timestamp = DateTime.Now });
                    }

                    await _context.SaveChangesAsync();
                    return transactionODTO;
                }
                else
                {
                    transaction.IssuerBankId = issuerBank.BankId;
                    transaction.TransactionStatus = Enums.TransactionStatus.ERROR;
                    transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.ERROR, Timestamp = DateTime.Now });
                    await _context.SaveChangesAsync();
                    return null;
                }
            }
            catch (Exception)
            {
                transaction.IssuerBankId = issuerBank.BankId;
                transaction.TransactionStatus = Enums.TransactionStatus.ERROR;
                transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.ERROR, Timestamp = DateTime.Now });
                await _context.SaveChangesAsync();
                return null;
            }
        }
    }
}
