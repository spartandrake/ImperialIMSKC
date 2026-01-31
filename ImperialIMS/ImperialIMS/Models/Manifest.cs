namespace ImperialIMS.Models
{
    public class Manifest : EntityBase
    {
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public int ShippingId { get; set; }
        public Shipment Shipment { get; set; }
        public int amount { get; set; }
    }
}
