using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ReportService
    {
        private readonly ItemRepo _itemRepo;
        private readonly ApplicationUserService _applicationUserService;
        private readonly InventoryItemRepo _inventoryItemRepo;
        private readonly ShipmentRepo _shipmentRepo;
        private readonly AlertRepo _alertRepo;
        public ReportService(ItemRepo itemRepo, ApplicationUserService applicationUserService, InventoryItemRepo inventoryItemRepo, ShipmentRepo shipmentRepo, AlertRepo alertRepo)
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
