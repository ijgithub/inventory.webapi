using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Inventory.WebApi.Migrations
{
    public partial class Propertynamechanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "WeaponTemplates");

            migrationBuilder.AddColumn<int>(
                name: "WeaponType",
                table: "WeaponTemplates",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeaponType",
                table: "WeaponTemplates");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "WeaponTemplates",
                nullable: false,
                defaultValue: 0);
        }
    }
}
