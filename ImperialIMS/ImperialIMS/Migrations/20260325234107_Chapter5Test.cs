using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperialIMS.Migrations
{
    /// <inheritdoc />
    public partial class Chapter5Test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Shipments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Alerts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Alerts");
        }
    }
}
