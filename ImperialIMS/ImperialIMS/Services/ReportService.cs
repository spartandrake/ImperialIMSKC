using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ReportService
    {
        private readonly IRepo<Item> _itemRepo;
        private readonly ApplicationUserService _applicationUserService;
        private readonly IRepo<InventoryItem> _inventoryItemRepo;
        private readonly IRepo<Shipment> _shipmentRepo;
        private readonly IRepo<Alert> _alertRepo;
        public ReportService(IRepo<Item> itemRepo, ApplicationUserService applicationUserService, IRepo<InventoryItem> inventoryItemRepo, IRepo<Shipment> shipmentRepo, IRepo<Alert> alertRepo)
        {
            _itemRepo = itemRepo;
            _applicationUserService = applicationUserService;
            _inventoryItemRepo = inventoryItemRepo;
            _shipmentRepo = shipmentRepo;
            _alertRepo = alertRepo;
        }
        public IEnumerable<InventoryItem> GetStorageFacilityInventory(int StorageFacilityId)
        {
            return _inventoryItemRepo.Search(x => x.StorageFacilityId == StorageFacilityId);
        }
        public IEnumerable<Alert> GetAlertsForShipments()
        {
            return _alertRepo.Search(x => x.ShipmentId != null);
        }
        public IEnumerable<Alert> GetAlertsForInventoryItems()
        {
            return _alertRepo.Search(x => x.InventoryItemId != null);
        }
    }
}
