using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperialIMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShipmentLocationToOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_StorageFacilities_DeliveryLocationId",
                table: "Shipments");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryLocationId",
                table: "Shipments",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_StorageFacilities_DeliveryLocationId",
                table: "Shipments",
                column: "DeliveryLocationId",
                principalTable: "StorageFacilities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shipments_StorageFacilities_DeliveryLocationId",
                table: "Shipments");

            migrationBuilder.AlterColumn<int>(
                name: "DeliveryLocationId",
                table: "Shipments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipments_StorageFacilities_DeliveryLocationId",
                table: "Shipments",
                column: "DeliveryLocationId",
                principalTable: "StorageFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
