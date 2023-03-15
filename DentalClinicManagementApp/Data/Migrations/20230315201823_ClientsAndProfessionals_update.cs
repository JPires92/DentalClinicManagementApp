using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinicManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ClientsAndProfessionals_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_PostalCodes_PostalCodeZipCode",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_PostalCodes_PostalCodeZipCode",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_PostalCodeZipCode",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PostalCodeZipCode",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PostalCodeZipCode",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "ZipCodeID",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "PostalCodeZipCode",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ZipCodeID",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "PostalCodeID",
                table: "Professionals",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCodeID",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_PostalCodeID",
                table: "Professionals",
                column: "PostalCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PostalCodeID",
                table: "Clients",
                column: "PostalCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_PostalCodes_PostalCodeID",
                table: "Clients",
                column: "PostalCodeID",
                principalTable: "PostalCodes",
                principalColumn: "ZipCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_PostalCodes_PostalCodeID",
                table: "Professionals",
                column: "PostalCodeID",
                principalTable: "PostalCodes",
                principalColumn: "ZipCode",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_PostalCodes_PostalCodeID",
                table: "Clients");

            migrationBuilder.DropForeignKey(
                name: "FK_Professionals_PostalCodes_PostalCodeID",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Professionals_PostalCodeID",
                table: "Professionals");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PostalCodeID",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PostalCodeID",
                table: "Professionals");

            migrationBuilder.DropColumn(
                name: "PostalCodeID",
                table: "Clients");

            migrationBuilder.AddColumn<string>(
                name: "PostalCodeZipCode",
                table: "Professionals",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCodeID",
                table: "Professionals",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCodeZipCode",
                table: "Clients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZipCodeID",
                table: "Clients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_PostalCodeZipCode",
                table: "Professionals",
                column: "PostalCodeZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PostalCodeZipCode",
                table: "Clients",
                column: "PostalCodeZipCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_PostalCodes_PostalCodeZipCode",
                table: "Clients",
                column: "PostalCodeZipCode",
                principalTable: "PostalCodes",
                principalColumn: "ZipCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Professionals_PostalCodes_PostalCodeZipCode",
                table: "Professionals",
                column: "PostalCodeZipCode",
                principalTable: "PostalCodes",
                principalColumn: "ZipCode");
        }
    }
}
