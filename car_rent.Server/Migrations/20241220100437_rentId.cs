using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    /// <inheritdoc />
    public partial class rentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RentId_in_company",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentId_in_company",
                table: "History");
        }
    }
}
