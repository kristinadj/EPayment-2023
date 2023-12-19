﻿using System.ComponentModel.DataAnnotations;

namespace Base.DTO.Input
{
    public class PaymentRequestIDTO
    {
        public int MerchantId { get; set; }

        public int Code { get; set; }

        [Required]
        [StringLength(30)]
        public string Secret { get; set; }
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

        public PaymentRequestIDTO(string secret, string currencyCode)
        {
            Secret = secret;
            CurrencyCode = currencyCode;
            TransactionSuccessUrl = string.Empty;
            TransactionFailureUrl = string.Empty;
            TransactionErrorUrl = string.Empty;
        }
    }
}
