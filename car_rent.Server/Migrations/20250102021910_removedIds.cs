using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    /// <inheritdoc />
    public partial class removedIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_AspNetUsers_User_ID",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_Company_ID",
                table: "History");

            migrationBuilder.RenameColumn(
                name: "User_ID",
                table: "History",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_History_User_ID",
                table: "History",
                newName: "IX_History_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_History_Company_ID",
                table: "History",
                column: "Company_ID");

            migrationBuilder.AddForeignKey(
                name: "FK_History_AspNetUsers_UserId",
                table: "History",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_History_AspNetUsers_UserId",
                table: "History");

            migrationBuilder.DropIndex(
                name: "IX_History_Company_ID",
                table: "History");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "History",
                newName: "User_ID");

            migrationBuilder.RenameIndex(
                name: "IX_History_UserId",
                table: "History",
                newName: "IX_History_User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_History_Company_ID",
                table: "History",
                column: "Company_ID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_History_AspNetUsers_User_ID",
                table: "History",
                column: "User_ID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
