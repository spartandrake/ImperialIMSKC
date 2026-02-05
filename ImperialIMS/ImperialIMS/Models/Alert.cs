using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.Models
{
    public enum AlertType
    {
        LowStock,
        Delay
    }
    public class Alert : EntityBase
    {
        public string Message { get; set; }
        public AlertType alertType { get; set; }
        [ForeignKey("InventoryItem")]
        public int? InventoryItemId { get; set; }
        public InventoryItem? InventoryItem { get; set; }
        [ForeignKey("Shipment")]
        public int? ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
