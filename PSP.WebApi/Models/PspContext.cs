﻿using Microsoft.EntityFrameworkCore;

namespace PSP.WebApi.Models
{
    public class PspContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentMethodMerchant> PaymentMethodMerchants { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<SubscriptionDetails> SubscriptionDetails { get; set; }

        public PspContext(DbContextOptions<PspContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
            });

            builder.Entity<Invoice>(entity =>
            {
                entity.HasIndex(x => x.ExternalInvoiceId);

                entity.HasOne(x => x.Merchant)
                    .WithMany(x => x.Invoices)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Transaction)
                    .WithOne(x => x.Invoice)
                    .HasForeignKey<Invoice>(x => x.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.InvoiceType)
                   .HasConversion<string>();
            });

            builder.Entity<Merchant>(entity =>
            {
                entity.HasIndex(x => x.MerchantExternalId);

                entity.HasIndex(x => new { x.ServiceName, x.MerchantExternalId }).IsUnique();
            });


            builder.Entity<PaymentMethod>(entity =>
            {
                entity.HasIndex(x => new { x.ServiceName, x.ServiceApiSufix }).IsUnique();

            });

            builder.Entity<PaymentMethodMerchant>(entity =>
            {
                entity.HasOne(x => x.PaymentMethod)
                    .WithMany()
                    .HasForeignKey(x => x.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Merchant)
                    .WithMany(x => x.PaymentMethods)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            builder.Entity<Transaction>(entity =>
            {
                entity.HasOne(x => x.PaymentMethod)
                    .WithMany()
                    .HasForeignKey(x => x.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();
            });

            builder.Entity<TransactionLog>(entity =>
            {
                entity.HasOne(x => x.Transaction)
                    .WithMany(x => x.TransactionLogs)
                    .HasForeignKey(x => x.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();
            });

            builder.Entity<SubscriptionDetails>(entity =>
            {
                entity.HasOne(x => x.Invoice)
                    .WithMany()
                    .HasForeignKey(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            #endregion
        }
    }
}
