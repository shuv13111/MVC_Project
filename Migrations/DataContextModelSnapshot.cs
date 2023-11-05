﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoeTrackr.Data;

#nullable disable

namespace ShoeTrackr.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShoeTrackr.Models.ItemModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("price")
                        .HasColumnType("int");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.Property<double>("rating")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("items");
                });

            modelBuilder.Entity("ShoeTrackr.Models.PurchaseModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("item_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("purchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("item_id");

                    b.ToTable("purchases");
                });

            modelBuilder.Entity("ShoeTrackr.Models.SaleItemModel", b =>
                {
                    b.Property<int>("SalesItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalesItemId"));

                    b.Property<int>("item_id")
                        .HasColumnType("int");

                    b.Property<int>("sale_id")
                        .HasColumnType("int");

                    b.Property<double>("sale_price")
                        .HasColumnType("float");

                    b.Property<int>("sale_quantity")
                        .HasColumnType("int");

                    b.HasKey("SalesItemId");

                    b.HasIndex("item_id");

                    b.HasIndex("sale_id");

                    b.ToTable("saleItemModels");
                });

            modelBuilder.Entity("ShoeTrackr.Models.SalesModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("date")
                        .HasColumnType("datetime2");

                    b.Property<int>("netPrice")
                        .HasColumnType("int");

                    b.Property<double>("netQuantity")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("sales");
                });

            modelBuilder.Entity("ShoeTrackr.Models.PurchaseModel", b =>
                {
                    b.HasOne("ShoeTrackr.Models.ItemModel", "Item")
                        .WithMany("purchaseModels")
                        .HasForeignKey("item_id")
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("ShoeTrackr.Models.SaleItemModel", b =>
                {
                    b.HasOne("ShoeTrackr.Models.ItemModel", "items")
                        .WithMany("SaleItemModels")
                        .HasForeignKey("item_id")
                        .IsRequired();

                    b.HasOne("ShoeTrackr.Models.SalesModel", "sales")
                        .WithMany("SaleItemModels")
                        .HasForeignKey("sale_id")
                        .IsRequired();

                    b.Navigation("items");

                    b.Navigation("sales");
                });

            modelBuilder.Entity("ShoeTrackr.Models.ItemModel", b =>
                {
                    b.Navigation("SaleItemModels");

                    b.Navigation("purchaseModels");
                });

            modelBuilder.Entity("ShoeTrackr.Models.SalesModel", b =>
                {
                    b.Navigation("SaleItemModels");
                });
#pragma warning restore 612, 618
        }
    }
}