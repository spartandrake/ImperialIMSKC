using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.Models
{
    public class Manifest : EntityBase
    {
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Item Item { get; set; }
        [ForeignKey("Shipment")]
        public int ShippingId { get; set; }
        public Shipment Shipment { get; set; }
        public int amount { get; set; }
    }
}
