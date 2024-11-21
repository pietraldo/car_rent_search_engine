using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddCars : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Car_ID",
                table: "Offers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Car_ID",
                table: "History",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Car_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Car_ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offers_Car_ID",
                table: "Offers",
                column: "Car_ID");

            migrationBuilder.CreateIndex(
                name: "IX_History_Car_ID",
                table: "History",
                column: "Car_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_History_Cars_Car_ID",
                table: "History",
                column: "Car_ID",
                principalTable: "Cars",
                principalColumn: "Car_ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Cars_Car_ID",
                table: "Offers",
                column: "Car_ID",
                principalTable: "Cars",
                principalColumn: "Car_ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_Cars_Car_ID",
                table: "History");

            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Cars_Car_ID",
                table: "Offers");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Offers_Car_ID",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_History_Car_ID",
                table: "History");

            migrationBuilder.DropColumn(
                name: "Car_ID",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "Car_ID",
                table: "History");
        }
    }
}
