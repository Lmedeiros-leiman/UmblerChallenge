﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UmbraChallenge.Data;

#nullable disable

namespace UmbraChallenge.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240903223747_InitialMigrationWithTrasactionsAndUserKeys")]
    partial class InitialMigrationWithTrasactionsAndUserKeys
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("UmbraChallenge.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "e4431a30-c08d-4861-9aff-db90e2170bdf",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "1eb4498c-8b26-4682-87c6-6d9550572719",
                            Email = "admin@localhost",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@LOCALHOST",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "76fd2ffa-911a-46d6-943d-3cfb39734d50",
                            TwoFactorEnabled = false,
                            UserName = "admin@localhost"
                        },
                        new
                        {
                            Id = "176403f3-37b3-410a-9214-ded296964ee8",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "7eaf8dbd-75ba-4bb4-a2f6-758fc54c0677",
                            Email = "john@localhost",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "JOHN@LOCALHOST",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "772c3ca0-2748-4a9a-92f7-fd9846051b3e",
                            TwoFactorEnabled = false,
                            UserName = "john@localhost"
                        },
                        new
                        {
                            Id = "649aa4e4-e88c-4132-baa2-f62922e77103",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "44359f4d-67c1-470f-bb76-6d1a3e4d9ce9",
                            Email = "maria@localhost",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "MARIA@LOCALHOST",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "303d4655-c2c9-4cd1-b4df-546639a8c89d",
                            TwoFactorEnabled = false,
                            UserName = "maria@localhost"
                        });
                });

            modelBuilder.Entity("UmbraChallenge.Data.Models.Transaction", b =>
                {
                    b.Property<string>("TransactionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("CancelationTransationTransactionId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReceiverKeyId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double>("TransferAmmount")
                        .HasColumnType("REAL");

                    b.HasKey("TransactionId");

                    b.HasIndex("CancelationTransationTransactionId");

                    b.HasIndex("ReceiverKeyId");

                    b.HasIndex("SenderId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("UmbraChallenge.Data.Models.UserTransferKey", b =>
                {
                    b.Property<string>("KeyId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("KeyType")
                        .HasColumnType("INTEGER");

                    b.Property<string>("KeyValue")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("KeyId");

                    b.HasIndex("UserId");

                    b.ToTable("UserTransferKeys");

                    b.HasData(
                        new
                        {
                            KeyId = "a146580d-17ab-440f-a037-618aca2f5e4b",
                            IsActive = true,
                            KeyType = 0,
                            KeyValue = "admin@localhost",
                            UserId = "e4431a30-c08d-4861-9aff-db90e2170bdf"
                        },
                        new
                        {
                            KeyId = "040e896f-47e0-4f3a-8478-3f4fd727c4d1",
                            IsActive = true,
                            KeyType = 0,
                            KeyValue = "john@localhost",
                            UserId = "176403f3-37b3-410a-9214-ded296964ee8"
                        },
                        new
                        {
                            KeyId = "e2c52d3b-e488-46ff-86c3-f76b3eaf3213",
                            IsActive = true,
                            KeyType = 0,
                            KeyValue = "maria@localhost",
                            UserId = "649aa4e4-e88c-4132-baa2-f62922e77103"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("UmbraChallenge.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("UmbraChallenge.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UmbraChallenge.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("UmbraChallenge.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UmbraChallenge.Data.Models.Transaction", b =>
                {
                    b.HasOne("UmbraChallenge.Data.Models.Transaction", "CancelationTransation")
                        .WithMany()
                        .HasForeignKey("CancelationTransationTransactionId");

                    b.HasOne("UmbraChallenge.Data.Models.UserTransferKey", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverKeyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UmbraChallenge.Data.Models.ApplicationUser", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CancelationTransation");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("UmbraChallenge.Data.Models.UserTransferKey", b =>
                {
                    b.HasOne("UmbraChallenge.Data.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
