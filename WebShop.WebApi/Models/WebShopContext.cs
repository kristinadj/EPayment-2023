using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShop.DTO.Enums;

namespace WebShop.WebApi.Models
{
    public class WebShopContext : IdentityUserContext<User>
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderLog> OrdersLogs { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }
        public DbSet<UserSubscriptionPlan> UserSubscriptionPlans { get; set; }
        public DbSet<MerchantOrder> MerchantOrders { get; set; }

        public WebShopContext(DbContextOptions<WebShopContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();
            });

            builder.Entity<Invoice>(entity =>
            {
                entity.HasOne(x => x.Merchant)
                    .WithMany(x => x.Invoices)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Invoices)
                    .HasForeignKey(x => x.UserId)
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

            builder.Entity<Item>(entity =>
            {
                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Merchant)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Merchant>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithOne()
                    .HasForeignKey<Merchant>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.OrderStatus)
                    .HasConversion<string>();
            });

            builder.Entity<MerchantOrder>(entity =>
            {
                entity.HasOne(x => x.Merchant)
                    .WithMany(x => x.MerchantOrders)
                    .HasForeignKey(x => x.MerchantId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Invoice)
                    .WithOne()
                    .HasForeignKey<MerchantOrder>(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.MerchantOrder)
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.MerchantOrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Item)
                    .WithMany()
                    .HasForeignKey(x => x.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OrderLog>(entity =>
            {
                entity.HasOne(x => x.Order)
                    .WithMany(x => x.OrderLogs)
                    .HasForeignKey(x => x.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(d => d.OrderStatus)
                    .HasConversion<string>();
            });

            builder.Entity<PaymentMethod>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();

            });

            builder.Entity<ShoppingCart>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithOne()
                    .HasForeignKey<ShoppingCart>(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasOne(x => x.ShoppingCart)
                    .WithMany(x => x.ShoppingCartItems)
                    .HasForeignKey(x => x.ShoppingCartId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Item)
                    .WithMany()
                    .HasForeignKey(x => x.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<SubscriptionPlan>(entity =>
            {
                entity.HasIndex(x => x.Name).IsUnique();

                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Transaction>(entity =>
            {
                entity.HasOne(x => x.PaymentMethod)
                    .WithMany(x => x.Transactions)
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

            builder.Entity<User>(entity =>
            {
                entity.HasIndex(x => x.Email).IsUnique();
            });

            builder.Entity<UserSubscriptionPlan>(entity =>
            {
                entity.HasOne(x => x.User)
                    .WithMany(x => x.UserSubscriptionPlans)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.SubscriptionPlan)
                    .WithMany(x => x.UserSubscriptionPlans)
                    .HasForeignKey(x => x.SubscriptionPlanId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Invoice)
                    .WithOne()
                    .HasForeignKey<UserSubscriptionPlan>(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            #region Database Initialization

            builder.Entity<Currency>().HasData(
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1 },
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            var merchantId = "408b89e8-e8e5-4b97-9c88-f19593d66378";
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<User>().HasData(
                new User("Law Publishing Web Shop")
                {
                    Id = merchantId,
                    Role = Role.MERCHANT,
                    Address = "123 Main Street",
                    PhoneNumber = "+1 555-123-4567",
                    PasswordHash = hasher.HashPassword(null!, "Admin123#"),
                    Email = "webshopadmin@lawpublishingagency.com",
                    NormalizedEmail = "WEBSHOPADMIN@LAWPUBLISHINGAGENCY.COM",
                });

            builder.Entity<Merchant>().HasData(
                new Merchant(merchantId) { MerchantId = 1, IsMasterMerchant = true }
                );

            builder.Entity<Item>().HasData(
                new Item("Legislative Drafting", "Professional drafting of legislative documents.") { ItemId = 1, MerchantId = 1, Price = 500, CurrencyId = 2 },
                new Item("Policy Analysis", "Thorough analysis of governmental policies.") { ItemId = 2, MerchantId = 1, Price = 800, CurrencyId = 2 },
                new Item("Regulatory Compliance Consultation", "Expert consultation on regulatory compliance.") { ItemId = 3, MerchantId = 1, Price = 700, CurrencyId = 2 },
                new Item("Legal Document Review", "Ensure accuracy with legal document reviews.") { ItemId = 4, MerchantId = 1, Price = 120, CurrencyId = 2 },
                new Item("Government Contracts Review", "Detailed review of government contracts.") { ItemId = 5, MerchantId = 1, Price = 600, CurrencyId = 2 },
                new Item("Constitutional Law Expertise", "Specialized expertise in constitutional law matters.") { ItemId = 6, MerchantId = 1, Price = 750, CurrencyId = 2 },
                new Item("Government Contracts Review", "Detailed review of government contracts.") { ItemId = 7, MerchantId = 1, Price = 850, CurrencyId = 2 },
                new Item("Public Policy Advocacy", "Advocacy services for public policy initiatives.") { ItemId = 8, MerchantId = 1, Price = 550, CurrencyId = 2 },
                new Item("Government Litigation Support", "Legal support during governmental litigation.") { ItemId = 9, MerchantId = 1, Price = 700, CurrencyId = 2 },
                new Item("Ethics and Compliance Training", "Training programs for governmental ethics and compliance.") { ItemId = 10, MerchantId = 1, Price = 750, CurrencyId = 2 }
                );

            var secondMerchantId = "2e87d106-2e43-4a19-bd4c-843920dcf3e9";
            builder.Entity<User>().HasData(
                new User("Legal Documents Agency")
                {
                    Id = secondMerchantId,
                    Role = Role.MERCHANT,
                    Address = "456 Oak Avenue",
                    PhoneNumber = "+1 555-987-6543",
                    PasswordHash = hasher.HashPassword(null!, "AgencyPass456$"),
                    Email = "agencyadmin@legaldocsagency.com",
                    NormalizedEmail = "AGENCYADMIN@LEGALDOCSAGENCY.COM",
                });

            builder.Entity<Merchant>().HasData(
                new Merchant(secondMerchantId) { MerchantId = 2, IsMasterMerchant = false }
                );

            builder.Entity<Item>().HasData(
                new Item("Contract Drafting Services", "Professional drafting of legal contracts.") { ItemId = 11, MerchantId = 2, Price = 600, CurrencyId = 2 },
                new Item("Trademark Registration", "Assistance with trademark registration.") { ItemId = 12, MerchantId = 2, Price = 900, CurrencyId = 2 },
                new Item("Patent Filing Support", "Support for filing patents.") { ItemId = 13, MerchantId = 2, Price = 750, CurrencyId = 2 },
                new Item("Legal Translations", "Accurate translations of legal documents.") { ItemId = 14, MerchantId = 2, Price = 150, CurrencyId = 2 },
                new Item("Corporate Governance Advisory", "Consultation on corporate governance.") { ItemId = 15, MerchantId = 2, Price = 700, CurrencyId = 2 },
                new Item("Legal Research Services", "Thorough legal research assistance.") { ItemId = 16, MerchantId = 2, Price = 550, CurrencyId = 2 },
                new Item("Data Privacy Compliance", "Ensuring compliance with data privacy laws.") { ItemId = 17, MerchantId = 2, Price = 800, CurrencyId = 2 },
                new Item("International Law Consultation", "Expertise in international legal matters.") { ItemId = 18, MerchantId = 2, Price = 850, CurrencyId = 2 },
                new Item("Dispute Resolution Services", "Assistance in resolving legal disputes.") { ItemId = 19, MerchantId = 2, Price = 700, CurrencyId = 2 },
                new Item("Legal Training Seminars", "Seminars on various legal topics.") { ItemId = 20, MerchantId = 2, Price = 750, CurrencyId = 2 }
                );

            builder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan("One-time Access (1 year)", "Access to the application for a duration of one year.") { SubscriptionPlanId = 1, Price = 300, CurrencyId = 2, DurationInDays = 365, AutomaticRenewel = false },
                new SubscriptionPlan("Annual Subscription", "Annual subscription with automatic renewal.") { SubscriptionPlanId = 2, Price = 250, CurrencyId = 2, DurationInDays = 365, AutomaticRenewel = true }
                );

            #endregion
        }
    }
}
