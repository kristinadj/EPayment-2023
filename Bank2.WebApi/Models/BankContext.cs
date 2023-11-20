using Microsoft.EntityFrameworkCore;

namespace Bank2.WebApi.Models
{
    public class BankContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BusinessCustomer> BusinessCustomer { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Account>(entity =>
            {
                entity.HasIndex(x => x.AccountNumber).IsUnique();

                entity.HasOne(x => x.Owner)
                    .WithMany(x => x.Accounts)
                    .HasForeignKey(x => x.OwnerId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<BusinessCustomer>(entity =>
            {
                entity.HasOne(x => x.Customer)
                    .WithOne()
                    .HasForeignKey<BusinessCustomer>(x => x.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Card>(entity =>
            {
                entity.HasOne(x => x.Account)
                    .WithMany(x => x.Cards)
                    .HasForeignKey(x => x.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Currency>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
            });

            builder.Entity<Customer>(entity =>
            {
            });

            builder.Entity<ExchangeRate>(entity =>
            {
                entity.HasIndex(x => new { x.FromCurrencyId, x.ToCurrencyId }).IsUnique();

                entity.HasOne(x => x.FromCurrency)
                    .WithMany()
                    .HasForeignKey(x => x.FromCurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ToCurrency)
                    .WithMany()
                    .HasForeignKey(x => x.ToCurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SenderAccount)
                    .WithMany(x => x.TransactionsAsSender)
                    .HasForeignKey(x => x.SenderAccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ReceiverAccount)
                    .WithMany(x => x.TransactionsAsReceiver)
                    .HasForeignKey(x => x.ReceiverAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
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

            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            builder.Entity<ExchangeRate>().HasData(
                new ExchangeRate { ExchangeRateId = 1, FromCurrencyId = 1, ToCurrencyId = 2, Rate = 0.0085 },
                new ExchangeRate { ExchangeRateId = 2, FromCurrencyId = 1, ToCurrencyId = 3, Rate = 0.0093 },
                new ExchangeRate { ExchangeRateId = 3, FromCurrencyId = 2, ToCurrencyId = 1, Rate = 116.94 },
                new ExchangeRate { ExchangeRateId = 4, FromCurrencyId = 2, ToCurrencyId = 3, Rate = 1.09 },
                new ExchangeRate { ExchangeRateId = 5, FromCurrencyId = 3, ToCurrencyId = 1, Rate = 107.53 },
                new ExchangeRate { ExchangeRateId = 6, FromCurrencyId = 3, ToCurrencyId = 2, Rate = 0.92 }
                ); ;

            // Merchant
            builder.Entity<Customer>().HasData(
                new Customer("LP AGENCY", "", "126 Palm Street", "+1 555-444-4567", "admin@lpagency.com")
                {
                    CustomerId = 1
                });

            builder.Entity<BusinessCustomer>().HasData(
                new BusinessCustomer("LPAPassword123!") // TODO: Encrypt
                {
                    BusinessCustomerId = 1,
                    CustomerId = 1
                });

            builder.Entity<Account>().HasData(
               new Account("987612340")
               {
                   AccountId = 1,
                   Balance = 14500,
                   CurrencyId = 1,
                   OwnerId = 1
               });

            // Buyer
            builder.Entity<Customer>().HasData(
                new Customer("Emily", "Smith", "198 Oak Street,", "+1 555-9324-7643", "emilysmith@email.com")
                {
                    CustomerId = 2
                });

            builder.Entity<Account>().HasData(
               new Account("1234098765")
               {
                   AccountId = 2,
                   Balance = 12700,
                   CurrencyId = 1,
                   OwnerId = 2
               });

            builder.Entity<Card>().HasData(
                new Card("EMILY SMITH", "2023 5678 9012 3456", "06/24")
                {
                    CardId = 1,
                    AccountId = 2,
                    CVV = 955
                }); ;

            #endregion
        }
    }
}
