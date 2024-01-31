using Base.DTO.Input;
using Base.DTO.Output;
using Microsoft.EntityFrameworkCore;
using PaymentCardCenter.WebApi.Models;

namespace PaymentCardCenter.WebApi.Services
{
    public interface ITransactionService
    {
        Task<PccAquirerTransactionODTO?> ResolvePaymentAsync(PccAquirerTransactionIDTO transactionIDTO);
        Task<PccAquirerTransactionODTO?> ResolvePaymentAsync(PccIssuerTransactionIDTO transactionIDTO);
    }

    public class TransactionService : ITransactionService
    {
        private readonly PccContext _context;

        public TransactionService(PccContext pccContext)
        {
            _context = pccContext;
        }

        public async Task<PccAquirerTransactionODTO?> ResolvePaymentAsync(PccAquirerTransactionIDTO transactionIDTO)
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
                var response = await httpClient.PostAsJsonAsync($"{issuerBank.RedirectUrl}/Transaction/PCC/ReceiveAquirerTransaction", transactionIDTO);
                if (response.IsSuccessStatusCode)
                {
                    var transactionODTO = await response.Content.ReadFromJsonAsync<PccAquirerTransactionODTO?>();
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

        public async Task<PccAquirerTransactionODTO?> ResolvePaymentAsync(PccIssuerTransactionIDTO transactionIDTO)
        {
            var currency = await _context.Currencies
                .Where(x => x.Code == transactionIDTO.CurrencyCode)
                .AsNoTracking()
                .FirstOrDefaultAsync();
            if (currency == null) return null;

            var issuerBank = await _context.Banks
                .Where(x => x.BankId == transactionIDTO.IssuerBankId)
                .FirstOrDefaultAsync();

            if (issuerBank == null) return null;

            var transaction = new Transaction
            {
                IssuerBankId = issuerBank.BankId,
                IssuerTransactionId = transactionIDTO.IssuerTransctionId,
                IssuerTimestamp = transactionIDTO.IssuerTimestamp,
                Amount = transactionIDTO.Amount,
                CurrencyId = currency.CurrencyId,
                TransactionStatus = Enums.TransactionStatus.CREATED,
                TransactionLogs = new List<TransactionLog>
                {
                    new() { TransactionStatus = Enums.TransactionStatus.CREATED, Timestamp = DateTime.Now }
                }
            };

            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();

            var aquirerBank = await _context.Banks
                .Where(x => transactionIDTO!.AcquirerAccountNumber.StartsWith(x.AccountNumberStartNumbers))
                .FirstOrDefaultAsync();

            if (aquirerBank == null)
            {
                transaction.TransactionStatus = Enums.TransactionStatus.ERROR;
                transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.ERROR, Timestamp = DateTime.Now });
                await _context.SaveChangesAsync();
                return null;
            }

            using var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.PostAsJsonAsync($"{aquirerBank.RedirectUrl}/Transaction/PCC/ReceiveIssuerTransaction", transactionIDTO);
                if (response.IsSuccessStatusCode)
                {
                    var transactionODTO = await response.Content.ReadFromJsonAsync<PccAquirerTransactionODTO?>();
                    if (transactionODTO != null && transactionODTO.IsSuccess)
                    {
                        transaction.AquirerBankId = aquirerBank.BankId;
                        transaction.AquirerTransctionId = transactionODTO!.IssuerTransactionId;
                        transaction.AquirerTimestamp = transactionODTO!.IssuerTimestamp;
                        transaction.TransactionStatus = Enums.TransactionStatus.COMPLETED;
                        transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.COMPLETED, Timestamp = DateTime.Now });
                    }
                    else
                    {
                        transaction.AquirerBankId = aquirerBank.BankId;
                        transaction.TransactionStatus = Enums.TransactionStatus.FAIL;
                        transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.FAIL, Timestamp = DateTime.Now });
                    }

                    await _context.SaveChangesAsync();
                    return transactionODTO;
                }
                else
                {
                    transaction.AquirerBankId = aquirerBank.BankId;
                    transaction.TransactionStatus = Enums.TransactionStatus.ERROR;
                    transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.ERROR, Timestamp = DateTime.Now });
                    await _context.SaveChangesAsync();
                    return null;
                }
            }
            catch (Exception)
            {
                transaction.AquirerBankId = aquirerBank.BankId;
                transaction.TransactionStatus = Enums.TransactionStatus.ERROR;
                transaction.TransactionLogs.Add(new TransactionLog { TransactionStatus = Enums.TransactionStatus.ERROR, Timestamp = DateTime.Now });
                await _context.SaveChangesAsync();
                return null;
            }
        }
    }
}
