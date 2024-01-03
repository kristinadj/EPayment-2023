using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;

namespace PayPalPaymentService.WebApi.Models
{
    public class PayPalServiceContext : DbContext
    {
        private readonly IEncryptionProvider _provider;
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceLog> InvoiceLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

        public PayPalServiceContext(DbContextOptions<PayPalServiceContext> options) : base(options)
        {
            var key = "tQL9h3wtpfAtU+k3R+QOkA==";
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


            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            // Merchant
            builder.Entity<Merchant>().HasData(
                new Merchant("AV3Z4kgHI5D0Dbmcx9Aad6ES4BNG2TgPSipgEEtc2x0sq44FjQcDD3nbbKd9swsAz2wuW_HLKu6L64uh", "EAnL13M1IeQPT2YTsdwgmrO1R9RI97mqWFnF7mD7WQULXL6fmiTWIn9pDI1X6aAdK6leDX0RLdjM7tDh") { MerchantId = 1, PaymentServiceMerchantId = 2 });

            builder.Entity<Merchant>().HasData(
                new Merchant(string.Empty, string.Empty) { MerchantId = 2, PaymentServiceMerchantId = 3 });

            #endregion
        }
    }
}
