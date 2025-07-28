using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRestorAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestoreBy",
                table: "Products");

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "Warehouses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "Suppliers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "Suppliers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "PurchaseInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "PurchaseInvoices",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "PurchaseInvoiceItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "PurchaseInvoiceItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "ProductInventories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "ProductInventories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestorAt",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RestorBy",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "Warehouses");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "Suppliers");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "PurchaseInvoices");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "PurchaseInvoiceItems");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "ProductInventories");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "ProductInventories");

            migrationBuilder.DropColumn(
                name: "RestorAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "RestorBy",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "RestoreBy",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
