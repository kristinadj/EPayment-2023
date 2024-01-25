using Bank1.WebApi.Helpers;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.EntityFrameworkCore;

namespace Bank1.WebApi.Models
{
    public class BankContext : DbContext
    {
        private readonly IEncryptionProvider _provider;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BusinessCustomer> BusinessCustomer { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<IssuerTransaction> IssuerTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<RecurringTransactionDefinition> RecurringTransactionDefinitions { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

        public BankContext(DbContextOptions<BankContext> options) : base(options)
        {
            var key = "tQL9h3wtpfAtU+k3R+QOkA==";
            _provider = new GenerateEncryptionProvider(key);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.UseEncryption(_provider);

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

                entity.HasOne(x => x.DefaultAccount)
                    .WithOne()
                    .HasForeignKey<BusinessCustomer>(x => x.DefaultAccountId)
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

            builder.Entity<IssuerTransaction>(entity =>
            {
                entity.Property(d => d.TransactionStatus)
                    .HasConversion<string>();

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.IsuerAccount)
                    .WithMany(x => x.TransactionsAsIssuer)
                    .HasForeignKey(x => x.IssuerAccountId)
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

            builder.Entity<RecurringTransactionDefinition>(entity =>
            {
                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ReceiverAccount)
                    .WithMany()
                    .HasForeignKey(x => x.ReceiverAccountId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<RecurringTransaction>(entity =>
            {
                entity.HasOne(x => x.Transaction)
                    .WithMany()
                    .HasForeignKey(x => x.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.RecurringTransactionDefinition)
                    .WithMany(x => x.RecurringTransactions)
                    .HasForeignKey(x => x.RecurringTransactionDefinitionId)
                    .OnDelete(DeleteBehavior.Restrict);
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
                new Customer("Web prodavnica pravnog izdavaštva", "", "123 Glavna ulica", "+1 555-123-4567", "webshopadmin@lawpublishingagency.com")
                {
                    CustomerId = 1
                });

            builder.Entity<Account>().HasData(
               new Account("105-0000000000000-29")
               {
                   AccountId = 1,
                   Balance = 14500,
                   CurrencyId = 1,
                   OwnerId = 1
               });

            builder.Entity<BusinessCustomer>().HasData(
                new BusinessCustomer("LPAPassword5!")
                {
                    BusinessCustomerId = 1,
                    CustomerId = 1,
                    DefaultAccountId = 1
                });

            // Buyer
            builder.Entity<Customer>().HasData(
                new Customer("John", "Doe", "789 Ulica jorgovana,", "+1 555-987-6543", "johndoe@email.com")
                {
                    CustomerId = 2
                });

            builder.Entity<Account>().HasData(
               new Account("106-0000000000000-30")
               {
                   AccountId = 2,
                   Balance = 6530,
                   CurrencyId = 1,
                   OwnerId = 2
               });

            builder.Entity<Card>().HasData(
                new Card("JOHN DOE", "1234 5678 9012 3456", "12/25")
                {
                    CardId = 1,
                    AccountId = 2,
                    CVV = 123
                }); ;

            #endregion
        }
    }
}
