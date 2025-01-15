using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace car_rent.Server.Migrations
{
    /// <inheritdoc />
    public partial class changingkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_History_Offers_Offer_ID",
                table: "History");

            // Drop primary key on the Offers table
            migrationBuilder.DropPrimaryKey(
                name: "PK_Offers",
                table: "Offers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_History",
                table: "History");

          

            // Alter the column in Offers table
            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Offers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Alter columns in History table
            migrationBuilder.AlterColumn<string>(
                name: "RentId_in_company",
                table: "History",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Offer_ID",
                table: "History",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "Rent_ID",
                table: "History",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            // Recreate the primary key on Offers table
            migrationBuilder.AddPrimaryKey(
                name: "PK_Offers",
                table: "Offers",
                column: "Id");

            // Recreate the foreign key on History table
            migrationBuilder.AddForeignKey(
                name: "FK_History_Offers_Offer_ID",
                table: "History",
                column: "Offer_ID",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddPrimaryKey(
               name: "PK_History",
               table: "History",
               column: "Rent_ID");


        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Offers",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "RentId_in_company",
                table: "History",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Offer_ID",
                table: "History",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Rent_ID",
                table: "History",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
