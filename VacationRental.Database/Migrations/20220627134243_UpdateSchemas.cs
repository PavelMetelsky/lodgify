using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationRental.Database.Migrations
{
    public partial class UpdateSchemas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Rentals",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PreparationTimeInDays",
                table: "Rentals",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Bookings",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "PreparationTimeInDays",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Bookings");
        }
    }
}
