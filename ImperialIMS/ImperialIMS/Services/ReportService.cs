using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.ViewModel;

namespace ImperialIMS.Services
{
    public class ReportService
    {
        private readonly IRepo<Item> _itemRepo;
        private readonly ApplicationUserService _applicationUserService;
        private readonly IRepo<InventoryItem> _inventoryItemRepo;
        private readonly IRepo<Shipment> _shipmentRepo;
        private readonly IRepo<Alert> _alertRepo;
        private readonly IRepo<InventoryHistory> _inventoryHistoryRepo;
        public ReportService(IRepo<Item> itemRepo, ApplicationUserService applicationUserService, IRepo<InventoryItem> inventoryItemRepo, IRepo<Shipment> shipmentRepo, IRepo<Alert> alertRepo, IRepo<InventoryHistory> inventoryHistoryRepo)
        {
            _itemRepo = itemRepo;
            _applicationUserService = applicationUserService;
            _inventoryItemRepo = inventoryItemRepo;
            _shipmentRepo = shipmentRepo;
            _alertRepo = alertRepo;
            _inventoryHistoryRepo = inventoryHistoryRepo;
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
        public List<InventoryItemWithHistory> GetInventoryHistoryChanges(int storageFacilityId, DateTime from, DateTime to)
        {
            var inventoryItems = _inventoryItemRepo.Search(ii => ii.StorageFacilityId == storageFacilityId).ToList();
            return inventoryItems.Select(item => new InventoryItemWithHistory
            {
                InventoryItem = _inventoryItemRepo.Find(item.InventoryItem.Id),
                ItemName = _itemRepo.Find(item.ItemId).Name,
                History = _inventoryHistoryRepo.Search(ih => ih.InventoryItemId == item.Id && ih.ChangedAt >= from && ih.ChangedAt <= to)
                .OrderByDescending(ih => ih.ChangedAt).ToList()
            }).ToList();
        }
    }
}
