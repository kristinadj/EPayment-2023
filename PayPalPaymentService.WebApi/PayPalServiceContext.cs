﻿using Microsoft.EntityFrameworkCore;
using PayPalPaymentService.WebApi.Models;

namespace PayPalPaymentService.WebApi
{
    public class PayPalServiceContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceLog> InvoiceLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

        public PayPalServiceContext(DbContextOptions<PayPalServiceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


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


            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            // Merchant
            builder.Entity<Merchant>().HasData(
                new Merchant("sb-m1oed28379074@business.example.com", ",.Do6<TD") { MerchantId = 1, PaymentServiceMerchantId = 1 });

            #endregion
        }
    }
}