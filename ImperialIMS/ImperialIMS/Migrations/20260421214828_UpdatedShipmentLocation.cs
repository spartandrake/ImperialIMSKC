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
                table: "Shipments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_DeliveryLocationId",
                table: "Shipments",
                column: "DeliveryLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_StorageFacilities_DeliveryLocationId",
                table: "Shipments",
                column: "DeliveryLocationId",
                principalTable: "StorageFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_StorageFacilities_DeliveryLocationId",
                table: "Shipments");

            migrationBuilder.DropIndex(
                name: "IX_Shipments_DeliveryLocationId",
                table: "Shipments");

            migrationBuilder.DropColumn(
                name: "DeliveryLocationId",
                table: "Shipments");
        }
    }
}
