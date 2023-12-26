﻿using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;

namespace BankPaymentService.WebApi.Models
{
    public class BankPaymentServiceContext : DbContext
    {
        private readonly IEncryptionProvider _provider;
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceLog> InvoiceLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

        public BankPaymentServiceContext(DbContextOptions<BankPaymentServiceContext> options) : base(options) 
        {
            var key = "QlT9h3wtpdaRU+k3R+QOkA==";
            _provider = new GenerateEncryptionProvider(key);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseEncryption(_provider);

            builder.Entity<Currency>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
            });

            builder.Entity<InvoiceLog>(entity =>
            {
                entity.HasOne(x => x.Invoice)
                    .WithMany(x => x.InvoiceLogs)
                    .HasForeignKey(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();
            });

            builder.Entity<Invoice>(entity =>
            {
                entity.HasOne(x => x.Merchant)
                    .WithMany(x => x.Invoices)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();
            });

            builder.Entity<Merchant>(entity =>
            {
                entity.HasOne(x => x.Bank)
                    .WithMany(x => x.Merchants)
                    .HasForeignKey(x => x.BankId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            builder.Entity<Bank>().HasData(
                new Bank("HSBC Bank", "https://localhost:7092/api") { BankId = 1, ExternalBankId = 1 },
                new Bank("Capital Bank", "https://localhost:7130/api") { BankId = 2, ExternalBankId = 1 }
                );

            // Merchant
            builder.Entity<Merchant>().HasData(
                new Merchant("105-0000000000000-29", "LPAPassword5!") { MerchantId = 1, PaymentServiceMerchantId = 2, BankMerchantId = 1, BankId = 1 });

            #endregion
        }
    }
}
