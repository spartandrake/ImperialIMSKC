namespace ImperialIMS.Models
{
    public enum ShippingStatus
    {
        InTransit,
        Delivered,
        Cancelled,
        Lost,
        Delayed
    }
    public class Shipment : EntityBase
    {
        public DateTime RequestDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        //Third party tracking ID not a database key
        public int TrackingId { get; set; }
        public ShippingStatus Status { get; set; }
        public int ApplicationUserId { get; set; }

    }
}
