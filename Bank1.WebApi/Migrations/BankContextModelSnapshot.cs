﻿// <auto-generated />
using System;
using Bank1.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bank1.WebApi.Migrations
{
    [DbContext(typeof(BankContext))]
    partial class BankContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bank1.WebApi.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Balance")
                        .HasColumnType("float");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("AccountId");

                    b.HasIndex("AccountNumber")
                        .IsUnique();

                    b.HasIndex("CurrencyId");

                    b.HasIndex("OwnerId");

                    b.ToTable("Accounts", "dbo");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.BusinessCustomer", b =>
                {
                    b.Property<int>("BusinessCustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BusinessCustomerId"), 1L, 1);

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("BusinessCustomerId");

                    b.HasIndex("CustomerId")
                        .IsUnique();

                    b.ToTable("BusinessCustomers", "dbo");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("Expiratorydate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PanNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CardId");

                    b.HasIndex("AccountId");

                    b.ToTable("Cards", "dbo");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Currency", b =>
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
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CustomerId");

                    b.ToTable("Customers", "dbo");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.ExchangeRate", b =>
                {
                    b.Property<int>("ExchangeRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExchangeRateId"), 1L, 1);

                    b.Property<int>("FromCurrencyId")
                        .HasColumnType("int");

                    b.Property<double>("Rate")
                        .HasColumnType("float");

                    b.Property<int>("ToCurrencyId")
                        .HasColumnType("int");

                    b.HasKey("ExchangeRateId");

                    b.HasIndex("ToCurrencyId");

                    b.HasIndex("FromCurrencyId", "ToCurrencyId")
                        .IsUnique();

                    b.ToTable("ExchangeRates", "dbo");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ReceiverAccountId")
                        .HasColumnType("int");

                    b.Property<int>("SenderAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("TransactionId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ReceiverAccountId");

                    b.HasIndex("SenderAccountId");

                    b.ToTable("Transactions", "dbo");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.TransactionLog", b =>
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

            modelBuilder.Entity("Bank1.WebApi.Models.Account", b =>
                {
                    b.HasOne("Bank1.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank1.WebApi.Models.Customer", "Owner")
                        .WithMany("Accounts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.BusinessCustomer", b =>
                {
                    b.HasOne("Bank1.WebApi.Models.Customer", "Customer")
                        .WithOne()
                        .HasForeignKey("Bank1.WebApi.Models.BusinessCustomer", "CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Card", b =>
                {
                    b.HasOne("Bank1.WebApi.Models.Account", "Account")
                        .WithMany("Cards")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.ExchangeRate", b =>
                {
                    b.HasOne("Bank1.WebApi.Models.Currency", "FromCurrency")
                        .WithMany()
                        .HasForeignKey("FromCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank1.WebApi.Models.Currency", "ToCurrency")
                        .WithMany()
                        .HasForeignKey("ToCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FromCurrency");

                    b.Navigation("ToCurrency");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Transaction", b =>
                {
                    b.HasOne("Bank1.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank1.WebApi.Models.Account", "ReceiverAccount")
                        .WithMany("TransactionsAsReceiver")
                        .HasForeignKey("ReceiverAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank1.WebApi.Models.Account", "SenderAccount")
                        .WithMany("TransactionsAsSender")
                        .HasForeignKey("SenderAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("ReceiverAccount");

                    b.Navigation("SenderAccount");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.TransactionLog", b =>
                {
                    b.HasOne("Bank1.WebApi.Models.Transaction", "Transaction")
                        .WithMany("TransactionLogs")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Account", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("TransactionsAsReceiver");

                    b.Navigation("TransactionsAsSender");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Customer", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Bank1.WebApi.Models.Transaction", b =>
                {
                    b.Navigation("TransactionLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
