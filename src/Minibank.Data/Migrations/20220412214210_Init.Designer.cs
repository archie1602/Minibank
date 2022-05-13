﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minibank.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Minibank.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220412214210_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Minibank.Data.BankAccounts.BankAccountDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<DateTime?>("CloseDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("close_date");

                    b.Property<int>("CurrencyType")
                        .HasColumnType("integer")
                        .HasColumnName("currency_type");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("boolean")
                        .HasColumnName("is_closed");

                    b.Property<DateTime>("OpenDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("open_date");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_bank_account");

                    b.ToTable("bank_account", (string)null);
                });

            modelBuilder.Entity("Minibank.Data.TransferHistorys.TransferHistoryDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<int>("CurrencyType")
                        .HasColumnType("integer")
                        .HasColumnName("currency_type");

                    b.Property<int>("FromAccountId")
                        .HasColumnType("integer")
                        .HasColumnName("from_account_id");

                    b.Property<int>("ToAccountId")
                        .HasColumnType("integer")
                        .HasColumnName("to_account_id");

                    b.HasKey("Id")
                        .HasName("pk_transfer_history");

                    b.ToTable("transfer_history", (string)null);
                });

            modelBuilder.Entity("Minibank.Data.Users.UserDbModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Login")
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.HasKey("Id")
                        .HasName("pk_user");

                    b.ToTable("user", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
