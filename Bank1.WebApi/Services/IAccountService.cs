﻿using Bank1.WebApi.Helpers;
using Bank1.WebApi.Models;
using Base.DTO.Input;
using Microsoft.EntityFrameworkCore;

namespace Bank1.WebApi.Services
{
    public interface IAccountService
    {
        Task<Account?> GetAccountByCreditCardAsync(PayTransactionIDTO payTransactionIDTO);
    }

    public class AccountService : IAccountService
    {
        private readonly BankContext _context;

        public AccountService(BankContext context)
        {
            _context = context;
        }

        public async Task<Account?> GetAccountByCreditCardAsync(PayTransactionIDTO payTransactionIDTO)
        {
            //var hashedPanNUmber = Converter.HashPanNumber(payTransactionIDTO.PanNumber);
            return await _context.Accounts
                .Where(x => x.Cards!.Any(x => x.CardHolderName == payTransactionIDTO.CardHolderName && x.PanNumber == payTransactionIDTO.PanNumber && x.ExpiratoryDate == payTransactionIDTO.ExpiratoryDate && x.CVV == payTransactionIDTO.CVV))
                .FirstOrDefaultAsync();
        }
    }
}
