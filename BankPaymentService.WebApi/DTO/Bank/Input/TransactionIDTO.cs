﻿using System.ComponentModel.DataAnnotations;

namespace BankPaymentService.WebApi.DTO.Bank.Input
{
    public class TransactionIDTO
    {
        public int SenderId { get; set; }

        [Required]
        [StringLength(30)]
        public string Secret { get; set; }
        public string AccountNumber { get; set; }
        public double Amount { get; set; }

        [Required]
        [StringLength(3)]
        public string CurrencyCode { get; set; }
        public int ExternalInvoiceId { get; set; }
        public DateTime Timestamp { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionSuccessUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionFailureUrl { get; set; }

        [Required]
        [StringLength(70)]
        public string TransactionErrorUrl { get; set; }

        public bool IsQrCodePayment { get; set; }

        [StringLength(70)]
        public string? RecurringTransactionSuccessUrl { get; set; }

        [StringLength(70)]
        public string? RecurringTransactionFailureUrl { get; set; }

        public TransactionIDTO(string secret, string accountNumber, string currencyCode)
        {
            Secret = secret;
            AccountNumber = accountNumber;
            CurrencyCode = currencyCode;
            TransactionSuccessUrl = string.Empty;
            TransactionFailureUrl = string.Empty;
            TransactionErrorUrl = string.Empty;
        }
    }
}
