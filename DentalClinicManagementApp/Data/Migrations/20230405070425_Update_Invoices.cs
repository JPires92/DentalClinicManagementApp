using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinicManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update_Invoices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Invoices",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Invoices");
        }
    }
}
