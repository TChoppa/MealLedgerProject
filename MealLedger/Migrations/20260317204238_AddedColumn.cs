using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealLedger.Migrations
{
    /// <inheritdoc />
    public partial class AddedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Preference",
                table: "LunchRegistrations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Preference",
                table: "LunchRegistrations");
        }
    }
}
