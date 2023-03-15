using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalClinicManagementApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Professionals_creation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Professionals",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NIF = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCodeID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCodeZipCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProfessionalRoleID = table.Column<int>(type: "int", nullable: false),
                    SpecialityID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professionals", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Professionals_PostalCodes_PostalCodeZipCode",
                        column: x => x.PostalCodeZipCode,
                        principalTable: "PostalCodes",
                        principalColumn: "ZipCode");
                    table.ForeignKey(
                        name: "FK_Professionals_ProfessionalRoles_ProfessionalRoleID",
                        column: x => x.ProfessionalRoleID,
                        principalTable: "ProfessionalRoles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Professionals_Specialities_SpecialityID",
                        column: x => x.SpecialityID,
                        principalTable: "Specialities",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_PostalCodeZipCode",
                table: "Professionals",
                column: "PostalCodeZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_ProfessionalRoleID",
                table: "Professionals",
                column: "ProfessionalRoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Professionals_SpecialityID",
                table: "Professionals",
                column: "SpecialityID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Professionals");
        }
    }
}
