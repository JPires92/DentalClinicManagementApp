using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinicManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MedicalAppointments_Creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicalAppointments",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateOfAppointment = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observations = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Performed = table.Column<bool>(type: "bit", nullable: false),
                    ClientID = table.Column<int>(type: "int", nullable: false),
                    ProfessionalID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalAppointments", x => x.ID);
                    table.ForeignKey(
                        name: "FK_MedicalAppointments_Clients_ClientID",
                        column: x => x.ClientID,
                        principalTable: "Clients",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_MedicalAppointments_Professionals_ProfessionalID",
                        column: x => x.ProfessionalID,
                        principalTable: "Professionals",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_ClientID",
                table: "MedicalAppointments",
                column: "ClientID");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalAppointments_ProfessionalID",
                table: "MedicalAppointments",
                column: "ProfessionalID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalAppointments");
        }
    }
}
