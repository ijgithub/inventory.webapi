﻿// <auto-generated />
using Inventory.WebApi.Data;
using Inventory.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Inventory.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180616214604_Added_Recipes_Prototype")]
    partial class Added_Recipes_Prototype
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Inventory.WebApi.Models.CraftingInput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CraftingIngredientId");

                    b.Property<int>("Quantity");

                    b.Property<int?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("CraftingIngredientId");

                    b.HasIndex("RecipeId");

                    b.ToTable("CraftingInput");
                });

            modelBuilder.Entity("Inventory.WebApi.Models.CraftingOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CraftedItemId");

                    b.Property<int>("Quantity");

                    b.Property<int?>("RecipeId");

                    b.HasKey("Id");

                    b.HasIndex("CraftedItemId");

                    b.HasIndex("RecipeId");

                    b.ToTable("CraftingOutput");
                });

            modelBuilder.Entity("Inventory.WebApi.Models.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Inventory");
                });

            modelBuilder.Entity("Inventory.WebApi.Models.ItemTemplate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ItemType");

                    b.Property<int>("MaterialType");

                    b.Property<string>("Name");

                    b.Property<int>("TemplateType");

                    b.Property<string>("Title");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.ToTable("ItemTemplates");
                });

            modelBuilder.Entity("Inventory.WebApi.Models.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Name");

                    b.Property<int>("Title");

                    b.HasKey("Id");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Inventory.WebApi.Models.CraftingInput", b =>
                {
                    b.HasOne("Inventory.WebApi.Models.ItemTemplate", "CraftingIngredient")
                        .WithMany()
                        .HasForeignKey("CraftingIngredientId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inventory.WebApi.Models.Recipe")
                        .WithMany("CraftingIngredients")
                        .HasForeignKey("RecipeId");
                });

            modelBuilder.Entity("Inventory.WebApi.Models.CraftingOutput", b =>
                {
                    b.HasOne("Inventory.WebApi.Models.ItemTemplate", "CraftedItem")
                        .WithMany()
                        .HasForeignKey("CraftedItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Inventory.WebApi.Models.Recipe")
                        .WithMany("CraftedItems")
                        .HasForeignKey("RecipeId");
                });
#pragma warning restore 612, 618
        }
    }
}