﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PSP.WebApi.Models;

#nullable disable

namespace PSP.WebApi.Migrations
{
    [DbContext(typeof(PspContext))]
    [Migration("20231219080444_IsMasterMerchant")]
    partial class IsMasterMerchant
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PSP.WebApi.Models.Currency", b =>
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

            modelBuilder.Entity("PSP.WebApi.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"), 1L, 1);

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("ExternalInvoiceId")
                        .HasColumnType("int");

                    b.Property<string>("IssuedToUserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<int>("TransactionId")
                        .HasColumnType("int");

                    b.HasKey("InvoiceId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ExternalInvoiceId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Invoices", "dbo");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Merchant", b =>
                {
                    b.Property<int>("MerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MerchantId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MerchantExternalId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("TransactionErrorUrl")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("TransactionFailureUrl")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("TransactionSuccessUrl")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("MerchantId");

                    b.HasIndex("MerchantExternalId");

                    b.HasIndex("ServiceName", "MerchantExternalId")
                        .IsUnique();

                    b.ToTable("Merchants", "dbo");
                });

            modelBuilder.Entity("PSP.WebApi.Models.PaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ServiceApiSufix")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("PaymentMethodId");

                    b.HasIndex("ServiceName", "ServiceApiSufix")
                        .IsUnique();

                    b.ToTable("PaymentMethods", "dbo");
                });

            modelBuilder.Entity("PSP.WebApi.Models.PaymentMethodMerchant", b =>
                {
                    b.Property<int>("PaymentMethodMerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PaymentMethodMerchantId"), 1L, 1);

                    b.Property<int>("Code")
                        .HasColumnType("int");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentMethodMerchantId");

                    b.HasIndex("MerchantId");

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("PaymentMethodMerchants", "dbo");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Transaction", b =>
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

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("TransactionId");

                    b.HasIndex("InvoiceId")
                        .IsUnique();

                    b.HasIndex("PaymentMethodId");

                    b.ToTable("Transactions", "dbo");
                });

            modelBuilder.Entity("PSP.WebApi.Models.TransactionLog", b =>
                {
                    b.Property<int>("TransactionLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionLogId"), 1L, 1);

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionId")
                        .HasColumnType("int");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("TransactionLogId");

                    b.HasIndex("TransactionId");

                    b.ToTable("TransactionLogs", "dbo");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Invoice", b =>
                {
                    b.HasOne("PSP.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PSP.WebApi.Models.Merchant", "Merchant")
                        .WithMany("Invoices")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("PSP.WebApi.Models.PaymentMethodMerchant", b =>
                {
                    b.HasOne("PSP.WebApi.Models.Merchant", "Merchant")
                        .WithMany("PaymentMethods")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PSP.WebApi.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Merchant");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Transaction", b =>
                {
                    b.HasOne("PSP.WebApi.Models.Invoice", "Invoice")
                        .WithOne("Transaction")
                        .HasForeignKey("PSP.WebApi.Models.Transaction", "InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PSP.WebApi.Models.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Invoice");

                    b.Navigation("PaymentMethod");
                });

            modelBuilder.Entity("PSP.WebApi.Models.TransactionLog", b =>
                {
                    b.HasOne("PSP.WebApi.Models.Transaction", "Transaction")
                        .WithMany("TransactionLogs")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Invoice", b =>
                {
                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Merchant", b =>
                {
                    b.Navigation("Invoices");

                    b.Navigation("PaymentMethods");
                });

            modelBuilder.Entity("PSP.WebApi.Models.Transaction", b =>
                {
                    b.Navigation("TransactionLogs");
                });
#pragma warning restore 612, 618
        }
    }
}