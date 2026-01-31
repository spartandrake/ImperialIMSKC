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
        public int? InventoryItemId { get; set; }
        public InventoryItem? InventoryItem { get; set; }
        public int? ShipmentId { get; set; }
        public Shipment? Shipment { get; set; }
    }
}
