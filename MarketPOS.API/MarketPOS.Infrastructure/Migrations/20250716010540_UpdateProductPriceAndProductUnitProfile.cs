using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductPriceAndProductUnitProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductUnitProfiles",
                table: "ProductUnitProfiles");

            migrationBuilder.DropIndex(
                name: "IX_ProductUnitProfiles_ProductId",
                table: "ProductUnitProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.DropIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductUnitProfiles");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "ProductUnitProfiles");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductUnitProfiles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductUnitProfiles");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "DeleteBy",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "ValidFrom",
                table: "ProductPrices");

            migrationBuilder.DropColumn(
                name: "ValidTo",
                table: "ProductPrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductUnitProfiles",
                table: "ProductUnitProfiles",
                column: "ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductUnitProfiles",
                table: "ProductUnitProfiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductUnitProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DeleteBy",
                table: "ProductUnitProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductUnitProfiles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductUnitProfiles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "ProductPrices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "DeleteBy",
                table: "ProductPrices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "ProductPrices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ProductPrices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ProductPrices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidFrom",
                table: "ProductPrices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidTo",
                table: "ProductPrices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductUnitProfiles",
                table: "ProductUnitProfiles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductPrices",
                table: "ProductPrices",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnitProfiles_ProductId",
                table: "ProductUnitProfiles",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductPrices_ProductId",
                table: "ProductPrices",
                column: "ProductId",
                unique: true);
        }
    }
}
