using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebShop.WebApi.Enums;

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
        public DbSet<PaymentMethodMerchant> PaymentMethodMerchants { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<UserSubscriptionPlan> UserSubscriptionPlans { get; set; }

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
                entity.HasOne(x => x.Order)
                    .WithOne(x => x.Invoice)
                    .HasForeignKey<Invoice>(x => x.OrderId)
                    .OnDelete(DeleteBehavior.Restrict);

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

                entity.HasOne(x => x.Invoice)
                    .WithOne(x => x.Order)
                    .HasForeignKey<Order>(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<OrderItem>(entity =>
            {
                entity.HasOne(x => x.Currency)
                    .WithMany()
                    .HasForeignKey(x => x.CurrencyId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Order)
                    .WithMany(x => x.OrderItems)
                    .HasForeignKey(x => x.OrderId)
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
            });

            builder.Entity<PaymentMethod>(entity =>
            {
                entity.HasIndex(x => x.Code).IsUnique();

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
                entity.HasOne(x => x.Invoice)
                    .WithOne(x => x.Transaction)
                    .HasForeignKey<Transaction>(x => x.InvoiceId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.PaymentMethod)
                    .WithMany(x => x.Transactions)
                    .HasForeignKey(x => x.PaymentMethodId)
                    .OnDelete(DeleteBehavior.Restrict);
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
                new Currency("Serbian Dinar", "RSD", "RSD") { CurrencyId = 1},
                new Currency("Euro", "EUR", "€") { CurrencyId = 2 },
                new Currency("American Dollar", "USD", "$") { CurrencyId = 3 }
                );

            var merchantId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<User>().HasData(
                new User("Law Publishing Web Shop")
                {
                    Id = merchantId,
                    Role = Role.MERCHANT,
                    PasswordHash = hasher.HashPassword(null, "Admin123#"),
                    Email = "webshopadmin@lawpublishingagency.com",
                    NormalizedEmail = "WEBSHOPADMIN@LAWPUBLISHINGAGENCY.COM",
                });

            builder.Entity<Merchant>().HasData(
                new Merchant(merchantId) { MerchantId = 1 }
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

            builder.Entity<SubscriptionPlan>().HasData(
                new SubscriptionPlan("One-time Access (1 year)", "Access to the application for a duration of one year.") { SubscriptionPlanId = 1, Price = 300, CurrencyId = 2, DurationInDays = 365, AutomaticRenewel = false},
                new SubscriptionPlan("Annual Subscription", "Annual subscription with automatic renewal.") { SubscriptionPlanId = 2, Price = 250, CurrencyId = 2, DurationInDays = 365, AutomaticRenewel = true }
                );

            #endregion
        }
    }
}
