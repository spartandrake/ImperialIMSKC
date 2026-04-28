using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImperialIMS.Migrations
{
    /// <inheritdoc />
    public partial class ManifestInventoryItemFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manifests_Items_ItemId",
                table: "Manifests");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "Manifests",
                newName: "InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Manifests_ItemId",
                table: "Manifests",
                newName: "IX_Manifests_InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manifests_InventoryItems_InventoryItemId",
                table: "Manifests",
                column: "InventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Manifests_InventoryItems_InventoryItemId",
                table: "Manifests");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "Manifests",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Manifests_InventoryItemId",
                table: "Manifests",
                newName: "IX_Manifests_ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Manifests_Items_ItemId",
                table: "Manifests",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
