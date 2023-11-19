using Microsoft.EntityFrameworkCore;

namespace BankPaymentService.WebApi.Models
{
    public class BankPaymentServiceContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceLog> InvoiceLogs { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Merchant> Merchants { get; set; }

        public BankPaymentServiceContext(DbContextOptions<BankPaymentServiceContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Bank>(entity =>
            {
                entity.HasIndex(x => x.ExternalBankId).IsUnique();
            });

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
                new Bank("HSBC Bank", "https://localhost:7092/") { BankId = 1, ExternalBankId = 1 }
                );

            // Merchant
            builder.Entity<Merchant>().HasData(
                new Merchant("9876543210") { MerchantId = 1, PaymentServiceMerchantId = 1, BankMerchantId = 1, BankId = 1 });

            #endregion
        }
    }
}
