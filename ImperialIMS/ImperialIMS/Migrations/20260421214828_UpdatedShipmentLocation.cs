using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperialIMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedShipmentLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryLocationId",
                table: "Shipmetns",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shipmetns_DeliveryLocationId",
                table: "Shipmetns",
                column: "DeliveryLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipmetns_StorageFacilities_DeliveryLocationId",
                table: "Shipmetns",
                column: "DeliveryLocationId",
                principalTable: "StorageFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipmetns_StorageFacilities_DeliveryLocationId",
                table: "Shipmetns");

            migrationBuilder.DropIndex(
                name: "IX_Shipmetns_DeliveryLocationId",
                table: "Shipmetns");

            migrationBuilder.DropColumn(
                name: "DeliveryLocationId",
                table: "Shipmetns");
        }
    }
}
