﻿// <auto-generated />
using System;
using Bank2.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bank2.WebApi.Migrations
{
    [DbContext(typeof(BankContext))]
    [Migration("20231226004002_HashedPan")]
    partial class HashedPan
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Bank2.WebApi.Models.Account", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountId"), 1L, 1);

                    b.Property<string>("AccountNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

                    b.HasData(
                        new
                        {
                            AccountId = 1,
                            AccountNumber = "123-0000009876123-40",
                            Balance = 14500.0,
                            CurrencyId = 1,
                            OwnerId = 1
                        },
                        new
                        {
                            AccountId = 2,
                            AccountNumber = "123-0000000040987-65",
                            Balance = 12700.0,
                            CurrencyId = 1,
                            OwnerId = 2
                        });
                });

            modelBuilder.Entity("Bank2.WebApi.Models.BusinessCustomer", b =>
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

                    b.HasData(
                        new
                        {
                            BusinessCustomerId = 1,
                            CustomerId = 1,
                            Password = "LPAPassword123!"
                        });
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Card", b =>
                {
                    b.Property<int>("CardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CardId"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("CVV")
                        .HasColumnType("int");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ExpiratoryDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PanNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardId");

                    b.HasIndex("AccountId");

                    b.ToTable("Cards", "dbo");

                    b.HasData(
                        new
                        {
                            CardId = 1,
                            AccountId = 2,
                            CVV = 955,
                            CardHolderName = "EMILY SMITH",
                            ExpiratoryDate = "06/24",
                            PanNumber = "abaaaf0d6042a0916087da6065f84e4c1614b23d513daec6f5effc8987887832"
                        });
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Currency", b =>
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

            modelBuilder.Entity("Bank2.WebApi.Models.Customer", b =>
                {
                    b.Property<int>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CustomerId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(70)
                        .HasColumnType("nvarchar(70)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.HasData(
                        new
                        {
                            CustomerId = 1,
                            Address = "126 Palm Street",
                            Email = "admin@lpagency.com",
                            FirstName = "LP AGENCY",
                            LastName = "",
                            PhoneNumber = "+1 555-444-4567"
                        },
                        new
                        {
                            CustomerId = 2,
                            Address = "198 Oak Street,",
                            Email = "emilysmith@email.com",
                            FirstName = "Emily",
                            LastName = "Smith",
                            PhoneNumber = "+1 555-9324-7643"
                        });
                });

            modelBuilder.Entity("Bank2.WebApi.Models.ExchangeRate", b =>
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

                    b.HasData(
                        new
                        {
                            ExchangeRateId = 1,
                            FromCurrencyId = 1,
                            Rate = 0.0085000000000000006,
                            ToCurrencyId = 2
                        },
                        new
                        {
                            ExchangeRateId = 2,
                            FromCurrencyId = 1,
                            Rate = 0.0092999999999999992,
                            ToCurrencyId = 3
                        },
                        new
                        {
                            ExchangeRateId = 3,
                            FromCurrencyId = 2,
                            Rate = 116.94,
                            ToCurrencyId = 1
                        },
                        new
                        {
                            ExchangeRateId = 4,
                            FromCurrencyId = 2,
                            Rate = 1.0900000000000001,
                            ToCurrencyId = 3
                        },
                        new
                        {
                            ExchangeRateId = 5,
                            FromCurrencyId = 3,
                            Rate = 107.53,
                            ToCurrencyId = 1
                        },
                        new
                        {
                            ExchangeRateId = 6,
                            FromCurrencyId = 3,
                            Rate = 0.92000000000000004,
                            ToCurrencyId = 2
                        });
                });

            modelBuilder.Entity("Bank2.WebApi.Models.IssuerTransaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("AquirerTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("AquirerTransactionId")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IssuerAccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("TransactionId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("IssuerAccountId");

                    b.ToTable("IssuerTransactions", "dbo");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Transaction", b =>
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

                    b.Property<DateTime?>("IssuerTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IssuerTransactionId")
                        .HasColumnType("int");

                    b.Property<int>("ReceiverAccountId")
                        .HasColumnType("int");

                    b.Property<int?>("SenderAccountId")
                        .HasColumnType("int");

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

                    b.HasKey("TransactionId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ReceiverAccountId");

                    b.HasIndex("SenderAccountId");

                    b.ToTable("Transactions", "dbo");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.TransactionLog", b =>
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

            modelBuilder.Entity("Bank2.WebApi.Models.Account", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank2.WebApi.Models.Customer", "Owner")
                        .WithMany("Accounts")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.BusinessCustomer", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Customer", "Customer")
                        .WithOne()
                        .HasForeignKey("Bank2.WebApi.Models.BusinessCustomer", "CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Card", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Account", "Account")
                        .WithMany("Cards")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.ExchangeRate", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Currency", "FromCurrency")
                        .WithMany()
                        .HasForeignKey("FromCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank2.WebApi.Models.Currency", "ToCurrency")
                        .WithMany()
                        .HasForeignKey("ToCurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("FromCurrency");

                    b.Navigation("ToCurrency");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.IssuerTransaction", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank2.WebApi.Models.Account", "IsuerAccount")
                        .WithMany("TransactionsAsIssuer")
                        .HasForeignKey("IssuerAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("IsuerAccount");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Transaction", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank2.WebApi.Models.Account", "ReceiverAccount")
                        .WithMany("TransactionsAsReceiver")
                        .HasForeignKey("ReceiverAccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Bank2.WebApi.Models.Account", "SenderAccount")
                        .WithMany("TransactionsAsSender")
                        .HasForeignKey("SenderAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Currency");

                    b.Navigation("ReceiverAccount");

                    b.Navigation("SenderAccount");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.TransactionLog", b =>
                {
                    b.HasOne("Bank2.WebApi.Models.Transaction", "Transaction")
                        .WithMany("TransactionLogs")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Account", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("TransactionsAsIssuer");

                    b.Navigation("TransactionsAsReceiver");

                    b.Navigation("TransactionsAsSender");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Customer", b =>
                {
                    b.Navigation("Accounts");
                });

            modelBuilder.Entity("Bank2.WebApi.Models.Transaction", b =>
                {
                    b.Navigation("TransactionLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
