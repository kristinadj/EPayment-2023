﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PayPalPaymentService.WebApi.Models;

#nullable disable

namespace PayPalPaymentService.WebApi.Migrations
{
    [DbContext(typeof(PayPalServiceContext))]
    [Migration("20231221193940_PaymentServiceMerchantId")]
    partial class PaymentServiceMerchantId
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.Currency", b =>
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

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.Invoice", b =>
                {
                    b.Property<int>("InvoiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceId"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("ExternalInvoiceId")
                        .HasColumnType("int");

                    b.Property<int>("MerchantId")
                        .HasColumnType("int");

                    b.Property<string>("PayPalOrderId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PayerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionErrorUrl")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("TransactionFailureUrl")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.Property<string>("TransactionSuccessUrl")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.HasKey("InvoiceId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Invoices", "dbo");
                });

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.InvoiceLog", b =>
                {
                    b.Property<int>("InvoiceLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InvoiceLogId"), 1L, 1);

                    b.Property<int>("InvoiceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("InvoiceLogId");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceLogs", "dbo");
                });

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.Merchant", b =>
                {
                    b.Property<int>("MerchantId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MerchantId"), 1L, 1);

                    b.Property<string>("ClientId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PaymentServiceMerchantId")
                        .HasColumnType("int");

                    b.Property<string>("Secret")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("MerchantId");

                    b.ToTable("Merchants", "dbo");

                    b.HasData(
                        new
                        {
                            MerchantId = 1,
                            ClientId = "AV3Z4kgHI5D0Dbmcx9Aad6ES4BNG2TgPSipgEEtc2x0sq44FjQcDD3nbbKd9swsAz2wuW_HLKu6L64uh",
                            PaymentServiceMerchantId = 2,
                            Secret = "EAnL13M1IeQPT2YTsdwgmrO1R9RI97mqWFnF7mD7WQULXL6fmiTWIn9pDI1X6aAdK6leDX0RLdjM7tDh"
                        });
                });

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.Invoice", b =>
                {
                    b.HasOne("PayPalPaymentService.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PayPalPaymentService.WebApi.Models.Merchant", "Merchant")
                        .WithMany("Invoices")
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.InvoiceLog", b =>
                {
                    b.HasOne("PayPalPaymentService.WebApi.Models.Invoice", "Invoice")
                        .WithMany("InvoiceLogs")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Invoice");
                });

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.Invoice", b =>
                {
                    b.Navigation("InvoiceLogs");
                });

            modelBuilder.Entity("PayPalPaymentService.WebApi.Models.Merchant", b =>
                {
                    b.Navigation("Invoices");
                });
#pragma warning restore 612, 618
        }
    }
}
