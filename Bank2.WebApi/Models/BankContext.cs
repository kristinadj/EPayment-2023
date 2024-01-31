using Bank2.WebApi.AppSettings;
using EntityFrameworkCore.EncryptColumn.Extension;
using EntityFrameworkCore.EncryptColumn.Interfaces;
using EntityFrameworkCore.EncryptColumn.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bank2.WebApi.Models
{
    public class BankContext : IdentityUserContext<Customer>
    {
        private readonly IEncryptionProvider _provider;
        public DbSet<Account> Accounts { get; set; }
        public DbSet<BusinessCustomer> BusinessCustomer { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchangeRate> ExchangeRates { get; set; }
        public DbSet<IssuerTransaction> IssuerTransactions { get; set; }
        public DbSet<AcqurierTransaction> AcqurierTransactions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<RecurringTransactionDefinition> RecurringTransactionDefinitions { get; set; }
        public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

        public BankContext(DbContextOptions<BankContext> options, IOptions<BankSettings> bankSettings) : base(options)
        {
            _provider = new GenerateEncryptionProvider(bankSettings.Value.EncriptionKey);
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
                entity.HasIndex(x => x.Email).IsUnique();
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
                entity.HasOne(x => x.Transaction)
                    .WithMany()
                    .HasForeignKey(x => x.TransactionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<AcqurierTransaction>(entity =>
            {
                entity.HasOne(x => x.Transaction)
                    .WithMany()
                    .HasForeignKey(x => x.TransactionId)
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

                entity.HasOne(x => x.IssuerAccount)
                    .WithMany(x => x.LocalTransactionsAsIssuer)
                    .HasForeignKey(x => x.IssuerAccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.AquirerAccount)
                    .WithMany(x => x.LocalTransactionsAsAquirer)
                    .HasForeignKey(x => x.AquirerAccountId)
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

                entity.HasOne(x => x.AquirerAccount)
                    .WithMany()
                    .HasForeignKey(x => x.AquirerAccountId)
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

            var hasher = new PasswordHasher<IdentityUser>();

            // Merchant
            var merchantId = "84a03478-02d8-41a6-9397-735c6358470b";
            builder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = merchantId,
                    PasswordHash = hasher.HashPassword(null!, "Pass123!"),
                    Name = "Web shop 2",
                    Address = "123 Glavna ulica",
                    PhoneNumber = "+1 555-123-4567",
                    Email = "webshop2@gmail.com",
                    NormalizedEmail = "webshop2@gmail.com".ToUpper(),
                });

            builder.Entity<Account>().HasData(
               new Account
               {
                   AccountNumber = "123-0000000000001-89",
                   OwnerId = merchantId,
                   AccountId = 1,
                   Balance = 0,
                   CurrencyId = 2
               });

            builder.Entity<BusinessCustomer>().HasData(
                new BusinessCustomer
                {
                    BusinessCustomerId = 1,
                    CustomerId = merchantId,
                    DefaultAccountId = 1,
                    SecretKey = "Pass123!"
                });

            // Buyer
            var buyerId = "5250d188-dfd8-4bf4-8329-ee5c59807939";
            builder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = buyerId,
                    PasswordHash = hasher.HashPassword(null!, "Pass123!"),
                    Name = "Ann Smith",
                    Address = "789 Ulica jorgovana",
                    PhoneNumber = "+1 555-987-6543",
                    Email = "annsmith@gmail.com",
                    NormalizedEmail = "annsmith@gmail.com".ToUpper(),
                });

            builder.Entity<Account>().HasData(
               new Account
               {
                   AccountNumber = "123-0000000001234-76",
                   OwnerId = buyerId,
                   AccountId = 2,
                   Balance = 50000,
                   CurrencyId = 3
               });

            builder.Entity<Card>().HasData(
                new Card("ANN SMITH", "2024 1234 5678 9012", "10/26")
                {
                    CardId = 1,
                    AccountId = 2,
                    CVV = 999
                }); ;

            #endregion
        }

        internal object Entry(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
