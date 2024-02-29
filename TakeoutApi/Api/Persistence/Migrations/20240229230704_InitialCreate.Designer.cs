﻿// <auto-generated />
using System;
using API.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Api.Persistence.Migrations
{
    [DbContext(typeof(EfContext))]
    [Migration("20240229230704_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("Api.Domain.Entities.MenuCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MenuCategories");
                });

            modelBuilder.Entity("Api.Domain.Entities.MenuItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .HasMaxLength(120)
                        .HasColumnType("TEXT");

                    b.Property<int>("MenuCategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SalePrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MenuCategoryId");

                    b.ToTable("MenuItems");
                });

            modelBuilder.Entity("Api.Domain.Entities.MenuOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MenuOptionGroupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("SalePrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MenuOptionGroupId");

                    b.ToTable("MenuOptions");
                });

            modelBuilder.Entity("Api.Domain.Entities.MenuOptionGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("MenuOptionGroups");
                });

            modelBuilder.Entity("MenuItemMenuOptionGroup", b =>
                {
                    b.Property<int>("MenuItemsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MenuOptionGroupsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("MenuItemsId", "MenuOptionGroupsId");

                    b.HasIndex("MenuOptionGroupsId");

                    b.ToTable("MenuItemMenuOptionGroup");
                });

            modelBuilder.Entity("Api.Domain.Entities.MenuItem", b =>
                {
                    b.HasOne("Api.Domain.Entities.MenuCategory", "MenuCategory")
                        .WithMany()
                        .HasForeignKey("MenuCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuCategory");
                });

            modelBuilder.Entity("Api.Domain.Entities.MenuOption", b =>
                {
                    b.HasOne("Api.Domain.Entities.MenuOptionGroup", "MenuOptionGroup")
                        .WithMany()
                        .HasForeignKey("MenuOptionGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MenuOptionGroup");
                });

            modelBuilder.Entity("MenuItemMenuOptionGroup", b =>
                {
                    b.HasOne("Api.Domain.Entities.MenuItem", null)
                        .WithMany()
                        .HasForeignKey("MenuItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Domain.Entities.MenuOptionGroup", null)
                        .WithMany()
                        .HasForeignKey("MenuOptionGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
