namespace ImperialIMS.Models
{
    public enum ShippingStatus
    {
        InTransit,
        Delivered,
        Cancelled,
        Lost,
        Delayed,
        Pending
    }
    public class Shipment : EntityBase
    {
        public DateTime RequestDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime EstimatedDeliveryDate { get; set; }
        //Third party tracking ID not a database key
        public int TrackingId { get; set; }
        public ShippingStatus Status { get; set; }
        public string ApplicationUserId { get; set; }
        public StorageFacility? DeliveryLocation { get; set; }
        public int? DeliveryLocationId { get; set; }

    }
}
