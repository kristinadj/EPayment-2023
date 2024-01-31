using Microsoft.EntityFrameworkCore;

namespace PaymentCardCenter.WebApi.Models
{
    public class PccContext : DbContext
    {
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public PccContext(DbContextOptions<PccContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
            });

            builder.Entity<Bank>(entity =>
            {
                entity.HasIndex(x => x.CardStartNumbers).IsUnique();
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AquirerBank)
                    .WithMany(x => x.AquirerTransactions)
                    .HasForeignKey(x => x.AquirerBankId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.IssuerBank)
                    .WithMany(x => x.IssuerTransaction)
                    .HasForeignKey(x => x.IssuerBankId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            builder.Entity<Bank>().HasData(
                new Bank("1234", "105", "https://localhost:7092/api") { BankId = 1 },
                new Bank("2023", "123", "https://localhost:7130/api") { BankId = 2 }
                );

            #endregion
        }
    }
}
