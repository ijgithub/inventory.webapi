using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inventory.WebApi.Migrations
{
    public partial class Added_Recipes_Prototype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<int>(nullable: false),
                    Title = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CraftingInput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CraftingIngredientId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftingInput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CraftingInput_ItemTemplates_CraftingIngredientId",
                        column: x => x.CraftingIngredientId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CraftingInput_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CraftingOutput",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CraftedItemId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftingOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CraftingOutput_ItemTemplates_CraftedItemId",
                        column: x => x.CraftedItemId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CraftingOutput_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CraftingInput_CraftingIngredientId",
                table: "CraftingInput",
                column: "CraftingIngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftingInput_RecipeId",
                table: "CraftingInput",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftingOutput_CraftedItemId",
                table: "CraftingOutput",
                column: "CraftedItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CraftingOutput_RecipeId",
                table: "CraftingOutput",
                column: "RecipeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CraftingInput");

            migrationBuilder.DropTable(
                name: "CraftingOutput");

            migrationBuilder.DropTable(
                name: "Recipes");
        }
    }
}
