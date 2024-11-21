﻿// <auto-generated />
using System;
using HD.Wallet.Account.Service.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HD.Wallet.Account.Service.Migrations
{
    [DbContext(typeof(HdWalletAccountDbContext))]
    [Migration("20241121145835_RemoveName")]
    partial class RemoveName
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.Accounts.AccountEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccountType")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsBankLinking")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUnlinked")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LinkedAccountId")
                        .HasColumnType("text");

                    b.Property<int>("TransactionLimit")
                        .HasColumnType("integer");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("WalletBalance")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Account", (string)null);
                });

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.SavedDestinations.SavedDestinationEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("AccountBankJson")
                        .HasColumnType("jsonb");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsBankLinking")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReferenceUserId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ReferenceUserId");

                    b.HasIndex("UserId");

                    b.ToTable("SavedDestination", (string)null);
                });

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.Users.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccountStatus")
                        .HasColumnType("integer");

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<string>("BackIdCardUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateOfExpiry")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FaceVerificationUrl")
                        .HasColumnType("text");

                    b.Property<string>("FrontIdCardUrl")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("HashPassword")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdCardNo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdCardType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsEkycVerfied")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Nationality")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PinPassword")
                        .HasColumnType("text");

                    b.Property<string>("PlaceOfOrigin")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PlaceOfResidence")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Sex")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.Accounts.AccountEntity", b =>
                {
                    b.HasOne("HD.Wallet.Account.Service.Infrastructure.Entities.Users.UserEntity", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("HD.Wallet.Account.Service.Infrastructure.Entities.Accounts.AccountBankValueObject", "AccountBank", b1 =>
                        {
                            b1.Property<string>("AccountEntityId")
                                .HasColumnType("text");

                            b1.Property<string>("BankAccountId")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("BankFullName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("BankName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("BankOwnerName")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("Bin")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("IdCardNo")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.Property<string>("LogoUrl")
                                .IsRequired()
                                .HasColumnType("text");

                            b1.HasKey("AccountEntityId");

                            b1.ToTable("Account");

                            b1.WithOwner()
                                .HasForeignKey("AccountEntityId");
                        });

                    b.Navigation("AccountBank")
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.SavedDestinations.SavedDestinationEntity", b =>
                {
                    b.HasOne("HD.Wallet.Account.Service.Infrastructure.Entities.Users.UserEntity", "ReferenceUser")
                        .WithMany("ReferencedUsers")
                        .HasForeignKey("ReferenceUserId");

                    b.HasOne("HD.Wallet.Account.Service.Infrastructure.Entities.Users.UserEntity", "User")
                        .WithMany("SavedDestinations")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReferenceUser");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.Users.UserEntity", b =>
                {
                    b.OwnsOne("HD.Wallet.Account.Service.Infrastructure.Entities.Users.AddressValueObject", "Address", b1 =>
                        {
                            b1.Property<string>("UserEntityId")
                                .HasColumnType("text");

                            b1.Property<string>("District")
                                .HasColumnType("text");

                            b1.Property<string>("ProvinceOrCity")
                                .HasColumnType("text");

                            b1.Property<string>("Street")
                                .HasColumnType("text");

                            b1.Property<string>("WardOrCommune")
                                .HasColumnType("text");

                            b1.HasKey("UserEntityId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserEntityId");
                        });

                    b.OwnsOne("HD.Wallet.Account.Service.Infrastructure.Entities.Users.WorkValueObject", "Work", b1 =>
                        {
                            b1.Property<string>("UserEntityId")
                                .HasColumnType("text");

                            b1.Property<string>("Occupation")
                                .HasColumnType("text");

                            b1.Property<string>("Position")
                                .HasColumnType("text");

                            b1.HasKey("UserEntityId");

                            b1.ToTable("User");

                            b1.WithOwner()
                                .HasForeignKey("UserEntityId");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("Work")
                        .IsRequired();
                });

            modelBuilder.Entity("HD.Wallet.Account.Service.Infrastructure.Entities.Users.UserEntity", b =>
                {
                    b.Navigation("Accounts");

                    b.Navigation("ReferencedUsers");

                    b.Navigation("SavedDestinations");
                });
#pragma warning restore 612, 618
        }
    }
}