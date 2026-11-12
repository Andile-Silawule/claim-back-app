using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClaimBack.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseReturnDeadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDeadline",
                table: "Purchases",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnDeadline",
                table: "Purchases");
        }
    }
}
