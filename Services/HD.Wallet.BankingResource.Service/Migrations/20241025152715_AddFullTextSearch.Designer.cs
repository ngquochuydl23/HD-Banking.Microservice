﻿// <auto-generated />
using System;
using HD.Wallet.BankingResource.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HD.Wallet.BankingResource.Service.Migrations
{
    [DbContext(typeof(BankingResourceDbContext))]
    [Migration("20241025152715_AddFullTextSearch")]
    partial class AddFullTextSearch
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");
            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("HD.Wallet.BankingResource.Service.Infrastructure.Entities.Bank", b =>
                {
                    b.Property<string>("Bin")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("AndroidAppId")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("LogoApp")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<int>("Top")
                        .HasColumnType("int(11)");

                    b.Property<double?>("_")
                        .HasColumnType("double")
                        .HasColumnName("#");

                    b.HasKey("Bin")
                        .HasName("PRIMARY");

                    b.HasIndex("Name", "ShortName", "Bin")
                        .HasAnnotation("Npgsql:IndexMethod", "FULLTEXT");

                    b.ToTable("Banks");
                });

            modelBuilder.Entity("HD.Wallet.BankingResource.Service.Infrastructure.Entities.CitizenAccountBank", b =>
                {
                    b.Property<string>("AccountNo")
                        .HasMaxLength(11)
                        .HasColumnType("varchar(11)");

                    b.Property<decimal>("Balance")
                        .HasPrecision(15, 2)
                        .HasColumnType("decimal(15,2)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Bin")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("IdCardNo")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("varchar(12)");

                    b.Property<DateOnly>("OpenedAt")
                        .HasColumnType("date");

                    b.Property<string>("OwnerName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("enum('Active','Inactive','Closed','Frozen')");

                    b.HasKey("AccountNo")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Bin" }, "FK_CitizenAccountBank_Banks");

                    b.ToTable("CitizenAccountBank", (string)null);
                });

            modelBuilder.Entity("HD.Wallet.BankingResource.Service.Infrastructure.Entities.CitizenAccountBank", b =>
                {
                    b.HasOne("HD.Wallet.BankingResource.Service.Infrastructure.Entities.Bank", "Bank")
                        .WithMany("CitizenAccountBanks")
                        .HasForeignKey("Bin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_CitizenAccountBank_Banks");

                    b.Navigation("Bank");
                });

            modelBuilder.Entity("HD.Wallet.BankingResource.Service.Infrastructure.Entities.Bank", b =>
                {
                    b.Navigation("CitizenAccountBanks");
                });
#pragma warning restore 612, 618
        }
    }
}
