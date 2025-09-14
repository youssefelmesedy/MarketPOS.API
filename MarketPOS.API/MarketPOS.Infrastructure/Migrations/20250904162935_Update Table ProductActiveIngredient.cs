using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableProductActiveIngredient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "ProductActiveIngredient");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "ProductActiveIngredient");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductActiveIngredient",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ProductActiveIngredient",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteBy",
                table: "ProductActiveIngredient",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductActiveIngredient",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductActiveIngredient",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductActiveIngredient",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedBy",
                table: "ProductActiveIngredient",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "ProductActiveIngredient",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "ProductActiveIngredient",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductActiveIngredient",
                type: "datetime2",
                nullable: true);
        }
    }
}
