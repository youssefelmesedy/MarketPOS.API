using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketPOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableIngredinent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ActiveIngredinentsProfile_ActiveIngredientsId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ActiveIngredinentsProfile");

            migrationBuilder.CreateTable(
                name: "ActiveIngredinents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RestorAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestorBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredinents", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ActiveIngredinents_ActiveIngredientsId",
                table: "Products",
                column: "ActiveIngredientsId",
                principalTable: "ActiveIngredinents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ActiveIngredinents_ActiveIngredientsId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "ActiveIngredinents");

            migrationBuilder.CreateTable(
                name: "ActiveIngredinentsProfile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeleteBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RestorAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestorBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveIngredinentsProfile", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ActiveIngredinentsProfile_ActiveIngredientsId",
                table: "Products",
                column: "ActiveIngredientsId",
                principalTable: "ActiveIngredinentsProfile",
                principalColumn: "Id");
        }
    }
}
