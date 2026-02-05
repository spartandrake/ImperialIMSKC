namespace ImperialIMS.Models
{
    public enum ShippingStatus
    {
        Pending,
        Shipped,
        InTransit,
        Delivered,
        Cancelled
    }
    public class Shipment : EntityBase
    {
        public DateTime RequestDate { get; set; }
        //Third party tracking ID not a database key
        public int TrackingId { get; set; }
        public ShippingStatus Status { get; set; }
    }
}
