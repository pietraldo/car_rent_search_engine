using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    /// <inheritdoc />
    public partial class companyinfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Insert first row
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Name", "Company_ID" },
                values: new object[] { "DriveEasy", "c3a21321-53c8-42b7-91d9-24cf8e05a683" });

            // Insert second row
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Name", "Company_ID" },
                values: new object[] { "CarRental", "c466b4b2-679a-4883-9f88-706958e392dc" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }

    }
}
