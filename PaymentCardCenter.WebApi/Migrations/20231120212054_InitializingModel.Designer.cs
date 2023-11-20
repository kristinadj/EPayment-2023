﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PaymentCardCenter.WebApi.Models;

#nullable disable

namespace PaymentCardCenter.WebApi.Migrations
{
    [DbContext(typeof(PccContext))]
    [Migration("20231120212054_InitializingModel")]
    partial class InitializingModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.Bank", b =>
                {
                    b.Property<int>("BankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankId"), 1L, 1);

                    b.Property<string>("CardStartNumbers")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RedirectUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BankId");

                    b.HasIndex("CardStartNumbers")
                        .IsUnique();

                    b.ToTable("Banks", "dbo");

                    b.HasData(
                        new
                        {
                            BankId = 1,
                            CardStartNumbers = "1234",
                            RedirectUrl = "https://localhost:7092/api"
                        },
                        new
                        {
                            BankId = 2,
                            CardStartNumbers = "2023",
                            RedirectUrl = "https://localhost:7130/api"
                        });
                });

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.Currency", b =>
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

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.Transaction", b =>
                {
                    b.Property<int>("Transactionid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Transactionid"), 1L, 1);

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("AquirerBankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AquirerTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int>("AquirerTransctionId")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int?>("IssuerBankId")
                        .HasColumnType("int");

                    b.Property<DateTime>("IssuerTimestamp")
                        .HasColumnType("datetime2");

                    b.Property<int?>("IssuerTransactionId")
                        .HasColumnType("int");

                    b.Property<string>("TransactionStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(24)");

                    b.HasKey("Transactionid");

                    b.HasIndex("AquirerBankId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("IssuerBankId");

                    b.ToTable("Transactions", "dbo");
                });

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.TransactionLog", b =>
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

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.Transaction", b =>
                {
                    b.HasOne("PaymentCardCenter.WebApi.Models.Bank", "AquirerBank")
                        .WithMany("AquirerTransactions")
                        .HasForeignKey("AquirerBankId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PaymentCardCenter.WebApi.Models.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PaymentCardCenter.WebApi.Models.Bank", "IssuerBank")
                        .WithMany("IssuerTransaction")
                        .HasForeignKey("IssuerBankId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("AquirerBank");

                    b.Navigation("Currency");

                    b.Navigation("IssuerBank");
                });

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.TransactionLog", b =>
                {
                    b.HasOne("PaymentCardCenter.WebApi.Models.Transaction", "Transaction")
                        .WithMany("TransactionLogs")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.Bank", b =>
                {
                    b.Navigation("AquirerTransactions");

                    b.Navigation("IssuerTransaction");
                });

            modelBuilder.Entity("PaymentCardCenter.WebApi.Models.Transaction", b =>
                {
                    b.Navigation("TransactionLogs");
                });
#pragma warning restore 612, 618
        }
    }
}
