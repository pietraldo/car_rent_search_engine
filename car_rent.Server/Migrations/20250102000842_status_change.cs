using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    /// <inheritdoc />
    public partial class status_change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE History SET Status = 0");
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "History",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
