using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ShipmentService : ServiceBase<Shipment>
    {
        private readonly ILogger<Shipment> _logger;
        private readonly IRepo<Shipment> _repo;
        private IConfiguration _configuration { get; set; }
        public ShipmentService(Repos.IRepo<Shipment> repo, IConfiguration configuration, ILogger<Shipment> logger) : base(repo, configuration, logger)
        {
            _repo = repo;
            _configuration = configuration;
            _logger = logger;
        }
        public List<Shipment> GetAllForUser(string userid)
        {
            return _repo.Search().Where(s => s.ApplicationUserId == userid).ToList();
        }
        public Shipment CreateShipmentForUser(string userId)
        {
            //need to verify that there is no pending shipment for this user before we create a new one
            
            var shipment = _repo.Search()
                .Where(s => s.ApplicationUserId == userId && s.Status == ShippingStatus.Pending)
                .FirstOrDefault();
            if (shipment != null) 
            {
                return shipment;
            }
            var newShipment = new Shipment
            {
                ApplicationUserId = userId,
                RequestDate = DateTime.UtcNow,
                Status = ShippingStatus.Pending,
                EstimatedDeliveryDate = DateTime.UtcNow.AddDays(7)
            };
            _repo.Add(newShipment);
            _repo.SaveChanges();
            _logger.LogInformation("Created new shipment with Id {ShipmentId} for user {UserId}.", newShipment.Id, newShipment.ApplicationUserId);
            return newShipment;
        }
        public void MarkShipmentAsReceived(int shipmentId)
        {
            var shipment = Get(shipmentId);
            if (shipment.Id == 0)
            {
                _logger.LogWarning("Attempted to mark shipment with Id {ShipmentId} as received, but it was not found.", shipmentId);
                return;
            }
            shipment.ReceivedDate = DateTime.UtcNow;
            shipment.Status = ShippingStatus.Delivered;
            _repo.Update(shipment);
            _repo.SaveChanges();
            _logger.LogInformation("Marked shipment with Id {ShipmentId} as received.", shipmentId);
        }
        public void MarkShipmentAsInTransit(int shipmentId, int trackingId)
        {
            var shipment = Get(shipmentId);
            if (shipment.Id == 0)
            {
                _logger.LogWarning("Attempted to mark shipment with Id {ShipmentId} as in transit, but it was not found.", shipmentId);
                return;
            }
            shipment.RequestDate = DateTime.UtcNow;
            shipment.TrackingId = trackingId;
            shipment.Status = ShippingStatus.InTransit;
            _repo.Update(shipment);
            _repo.SaveChanges();
            _logger.LogInformation("Marked shipment with Id {ShipmentId} as in transit.", shipmentId);
        }
        public void MarkShipmentAsCancelled(int shipmentId)
        {
            var shipment = Get(shipmentId);
            if (shipment.Id == 0)
            {
                _logger.LogWarning("Attempted to mark shipment with Id {ShipmentId} as cancelled, but it was not found.", shipmentId);
                return;
            }
            shipment.Status = ShippingStatus.Cancelled;
            _repo.Update(shipment);
            _repo.SaveChanges();
            _logger.LogInformation("Marked shipment with Id {ShipmentId} as cancelled.", shipmentId);
        }
        public void MarkShipmentAsLost(int shipmentId)
        {
            var shipment = Get(shipmentId);
            if (shipment.Id == 0)
            {
                _logger.LogWarning("Attempted to mark shipment with Id {ShipmentId} as lost, but it was not found.", shipmentId);
                return;
            }
            shipment.Status = ShippingStatus.Lost;
            _repo.Update(shipment);
            _repo.SaveChanges();
            _logger.LogInformation("Marked shipment with Id {ShipmentId} as lost.", shipmentId);
            //We need to escalate this to the sector commander
        }
    }
}
