using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventories_Products_ProductId",
                table: "ProductInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventories_Products_ProductId1",
                table: "ProductInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventories_Warehouses_WarehouseId",
                table: "ProductInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventories_Warehouses_WarehouseId1",
                table: "ProductInventories");

            migrationBuilder.DropIndex(
                name: "IX_ProductInventories_ProductId1",
                table: "ProductInventories");

            migrationBuilder.DropIndex(
                name: "IX_ProductInventories_WarehouseId1",
                table: "ProductInventories");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductInventories");

            migrationBuilder.DropColumn(
                name: "WarehouseId1",
                table: "ProductInventories");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventories_Products_ProductId",
                table: "ProductInventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventories_Warehouses_WarehouseId",
                table: "ProductInventories",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventories_Products_ProductId",
                table: "ProductInventories");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductInventories_Warehouses_WarehouseId",
                table: "ProductInventories");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId1",
                table: "ProductInventories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId1",
                table: "ProductInventories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_ProductId1",
                table: "ProductInventories",
                column: "ProductId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInventories_WarehouseId1",
                table: "ProductInventories",
                column: "WarehouseId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventories_Products_ProductId",
                table: "ProductInventories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventories_Products_ProductId1",
                table: "ProductInventories",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventories_Warehouses_WarehouseId",
                table: "ProductInventories",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductInventories_Warehouses_WarehouseId1",
                table: "ProductInventories",
                column: "WarehouseId1",
                principalTable: "Warehouses",
                principalColumn: "Id");
        }
    }
}
