﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebShop.WebApi.Models;

#nullable disable

namespace WebShop.WebApi.Migrations
{
    [DbContext(typeof(WebShopContext))]
    partial class WebShopContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CurrencyId"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("CurrencyId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("Currencies", "dbo");

                    b.HasData(
                        new
                        {
                            CurrencyId = 1,
                            Code = "RSD",
                            Name = "Serbian Dinar",
                            Symbol = "RSD"
                        },
                        new
                        {
                            CurrencyId = 2,
                            Code = "EUR",
                            Name = "Euro",
                            Symbol = "€"
                        },
                        new
                        {
                            CurrencyId = 3,
                            Code = "USD",
                            Name = "American Dollar",
                            Symbol = "$"
                        });
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"), 1L, 1);

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int>("TransactionId")
                        .HasColumnType("int");

                    b.HasKey("InvoiceId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Invoices", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemId"), 1L, 1);

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ItemId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Items", "dbo");

                    b.HasData(
                        new
                        {
                            ItemId = 1,
                            CurrencyId = 2,
                            Description = "Professional drafting of legislative documents.",
                            MerchantId = 1,
                            Name = "Legislative Drafting",
                            Price = 500.0
                        },
                        new
                        {
                            ItemId = 2,
                            CurrencyId = 2,
                            Description = "Thorough analysis of governmental policies.",
                            MerchantId = 1,
                            Name = "Policy Analysis",
                            Price = 800.0
                        },
                        new
                        {
                            ItemId = 3,
                            CurrencyId = 2,
                            Description = "Expert consultation on regulatory compliance.",
                            MerchantId = 1,
                            Name = "Regulatory Compliance Consultation",
                            Price = 700.0
                        },
                        new
                        {
                            ItemId = 4,
                            CurrencyId = 2,
                            Description = "Ensure accuracy with legal document reviews.",
                            MerchantId = 1,
                            Name = "Legal Document Review",
                            Price = 120.0
                        },
                        new
                        {
                            ItemId = 5,
                            CurrencyId = 2,
                            Description = "Detailed review of government contracts.",
                            MerchantId = 1,
                            Name = "Government Contracts Review",
                            Price = 600.0
                        },
                        new
                        {
                            ItemId = 6,
                            CurrencyId = 2,
                            Description = "Specialized expertise in constitutional law matters.",
                            MerchantId = 1,
                            Name = "Constitutional Law Expertise",
                            Price = 750.0
                        },
                        new
                        {
                            ItemId = 7,
                            CurrencyId = 2,
                            Description = "Detailed review of government contracts.",
                            MerchantId = 1,
                            Name = "Government Contracts Review",
                            Price = 850.0
                        },
                        new
                        {
                            ItemId = 8,
                            CurrencyId = 2,
                            Description = "Advocacy services for public policy initiatives.",
                            MerchantId = 1,
                            Name = "Public Policy Advocacy",
                            Price = 550.0
                        },
                        new
                        {
                            ItemId = 9,
                            CurrencyId = 2,
                            Description = "Legal support during governmental litigation.",
                            MerchantId = 1,
                            Name = "Government Litigation Support",
                            Price = 700.0
                        },
                        new
                        {
                            ItemId = 10,
                            CurrencyId = 2,
                            Description = "Training programs for governmental ethics and compliance.",
                            MerchantId = 1,
                            Name = "Ethics and Compliance Training",
                            Price = 750.0
                        });
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Merchant", b =>
                {
                    b.Property<int>("MerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MerchantId"), 1L, 1);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("MerchantId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("Merchants", "dbo");

                    b.HasData(
                        new
                        {
                            MerchantId = 1,
                            UserId = "408b89e8-e8e5-4b97-9c88-f19593d66378"
                        });
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderId"), 1L, 1);

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("OrderId");

                    b.HasIndex("InvoiceId")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Orders", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.OrderItem", b =>
                {
                    b.Property<int>("OrderItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderItemId"), 1L, 1);

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderItemId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ItemId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderItems", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.OrderLog", b =>
                {
                    b.Property<int>("OrderLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OrderLogId"), 1L, 1);

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("OrderLogId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderLogs", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PaymentMethodId");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("PaymentMethods", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.PaymentMethodMerchant", b =>
                {
                    b.Property<int>("PaymentMethodMerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodMerchantId"), 1L, 1);

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.HasKey("PaymentMethodMerchantId");

                    b.HasIndex("MerchantId");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("PaymentMethodMerchants", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.ShoppingCart", b =>
                {
                    b.Property<int>("ShoppingCartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartId"), 1L, 1);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ShoppingCartId");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ShoppingCarts", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ShoppingCartItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ShoppingCartItemId"), 1L, 1);

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("ShoppingCartId")
                        .HasColumnType("int");

                    b.HasKey("ShoppingCartItemId");

                    b.HasIndex("ItemId");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingCartItems", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.SubscriptionPlan", b =>
                {
                    b.Property<int>("SubscriptionPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubscriptionPlanId"), 1L, 1);

                    b.Property<bool>("AutomaticRenewel")
                        .HasColumnType("bit");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("DurationInDays")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("SubscriptionPlanId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("SubscriptionPlans", "dbo");

                    b.HasData(
                        new
                        {
                            SubscriptionPlanId = 1,
                            AutomaticRenewel = false,
                            CurrencyId = 2,
                            Description = "Access to the application for a duration of one year.",
                            DurationInDays = 365,
                            Name = "One-time Access (1 year)",
                            Price = 300.0
                        },
                        new
                        {
                            SubscriptionPlanId = 2,
                            AutomaticRenewel = true,
                            CurrencyId = 2,
                            Description = "Annual subscription with automatic renewal.",
                            DurationInDays = 365,
                            Name = "Annual Subscription",
                            Price = 250.0
                        });
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<DateTime>("CreatedTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("TransactionStatus")
                        .HasColumnType("int");

                    b.HasKey("TransactionId");

                    b.HasIndex("InvoiceId")
                        .IsUnique();

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("Transactions", "dbo");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "408b89e8-e8e5-4b97-9c88-f19593d66378",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "adaa2276-290d-4d89-9bc7-4a6d90424ab7",
                            Email = "webshopadmin@lawpublishingagency.com",
                            EmailConfirmed = false,
                            LockoutEnabled = false,
                            Name = "Law Publishing Web Shop",
                            NormalizedEmail = "WEBSHOPADMIN@LAWPUBLISHINGAGENCY.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAEHx++6UzBh+IKkinQcNXoLkTNqAT1j0sLS8TYsyYIU6dJi1xGFwtvHOUfhAUbbqT7w==",
                            PhoneNumberConfirmed = false,
                            Role = 0,
                            SecurityStamp = "a5f89687-76e3-4d5d-8246-6187be0403c1",
                            TwoFactorEnabled = false,
                            UserName = "web-shop-admin"
                        });
                });

            modelBuilder.Entity("WebShop.WebApi.Models.UserSubscriptionPlan", b =>
                {
                    b.Property<int>("UserSubscriptionPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserSubscriptionPlanId"), 1L, 1);

                    b.Property<DateTime>("EndTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StartTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("SubscriptionPlanId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserSubscriptionPlanId");

                    b.HasIndex("InvoiceId")
                        .IsUnique();

                    b.HasIndex("SubscriptionPlanId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSubscriptionPlans", "dbo");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Invoice", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.Merchant", "Merchant")
                        .WithMany("Invoices")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Item", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.Merchant", "Merchant")
                        .WithMany("Items")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Merchant", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("WebShop.WebApi.Models.Merchant", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Order", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Invoice", "Invoice")
                        .WithOne("Order")
                        .HasForeignKey("WebShop.WebApi.Models.Order", "InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.OrderItem", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Item");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.OrderLog", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Order", "Order")
                        .WithMany("OrderLogs")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Order");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.PaymentMethodMerchant", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Merchant", "Merchant")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Merchant");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.ShoppingCart", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.User", "User")
                        .WithOne()
                        .HasForeignKey("WebShop.WebApi.Models.ShoppingCart", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.ShoppingCart", "ShoppingCart")
                        .WithMany("ShoppingCartItems")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("ShoppingCart");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.SubscriptionPlan", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Transaction", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Invoice", "Invoice")
                        .WithOne("Transaction")
                        .HasForeignKey("WebShop.WebApi.Models.Transaction", "InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.PaymentMethod", "PaymentMethod")
                        .WithMany("Transactions")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.UserSubscriptionPlan", b =>
                {
                    b.HasOne("WebShop.WebApi.Models.Invoice", "Invoice")
                        .WithOne()
                        .HasForeignKey("WebShop.WebApi.Models.UserSubscriptionPlan", "InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.SubscriptionPlan", "SubscriptionPlan")
                        .WithMany("UserSubscriptionPlans")
                        .HasForeignKey("SubscriptionPlanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebShop.WebApi.Models.User", "User")
                        .WithMany("UserSubscriptionPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("SubscriptionPlan");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Invoice", b =>
                {
                    b.Navigation("Order");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Merchant", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("Items");

                    b.Navigation("PaymentMethods");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.Order", b =>
                {
                    b.Navigation("OrderItems");

                    b.Navigation("OrderLogs");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.PaymentMethod", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingCartItems");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.SubscriptionPlan", b =>
                {
                    b.Navigation("UserSubscriptionPlans");
                });

            modelBuilder.Entity("WebShop.WebApi.Models.User", b =>
                {
                    b.Navigation("UserSubscriptionPlans");
                });
#pragma warning restore 612, 618
        }
    }
}
