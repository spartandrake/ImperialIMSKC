using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.Models
{
    public class Manifest : EntityBase
    {
        [ForeignKey("InventoryItem")]
        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
        [ForeignKey("Shipment")]
        public int ShippingId { get; set; }
        public Shipment Shipment { get; set; }
        public int amount { get; set; }
    }
}
