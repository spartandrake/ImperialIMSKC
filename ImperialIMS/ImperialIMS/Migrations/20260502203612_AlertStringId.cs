using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperialIMS.Migrations
{
    /// <inheritdoc />
    public partial class AlertStringId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                table: "Alerts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ApplicationUserId",
                table: "Alerts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
