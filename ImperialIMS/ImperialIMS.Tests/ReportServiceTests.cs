using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;

namespace ImperialIMS.Tests
{
    public class ReportServiceTests
    {
        private readonly IRepo<Item> _itemRepo;
        private readonly IRepo<InventoryItem> _inventoryItemRepo;
        private readonly IRepo<Shipment> _shipmentRepo;
        private readonly IRepo<Alert> _alertRepo;
        private readonly IRepo<InventoryHistory> _inventoryHistoryRepo;
        private readonly ReportService _svc;

        public ReportServiceTests()
        {
            _itemRepo = Substitute.For<IRepo<Item>>();
            _inventoryItemRepo = Substitute.For<IRepo<InventoryItem>>();
            _shipmentRepo = Substitute.For<IRepo<Shipment>>();
            _alertRepo = Substitute.For<IRepo<Alert>>();
            _inventoryHistoryRepo = Substitute.For<IRepo<InventoryHistory>>();

            var store = Substitute.For<IUserStore<ApplicationUser>>();
            var userManager = Substitute.For<UserManager<ApplicationUser>>(
                store, null, null, null, null, null, null, null, null);
            var appUserLogger = Substitute.For<ILogger<ApplicationUserService>>();
            var appUserService = new ApplicationUserService(userManager, appUserLogger);

            _svc = new ReportService(_itemRepo, appUserService, _inventoryItemRepo, _shipmentRepo, _alertRepo, _inventoryHistoryRepo);
        }

        [Fact]
        public void GetStorageFacilityInventory_Returns_Items_For_Facility()
        {
            var items = new List<InventoryItem>
            {
                new InventoryItem { Id = 1, StorageFacilityId = 5, StockCount = 10 },
                new InventoryItem { Id = 2, StorageFacilityId = 5, StockCount = 20 }
            }.AsQueryable();
            _inventoryItemRepo.Search(Arg.Any<Expression<Func<InventoryItem, bool>>>()).Returns(items);

            var result = _svc.GetStorageFacilityInventory(5);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetAlertsForShipments_Returns_Alerts_With_ShipmentId()
        {
            var alerts = new List<Alert>
            {
                new Alert { Id = 1, ShipmentId = 10, Message = "Delay" },
                new Alert { Id = 2, ShipmentId = 20, Message = "Delay" }
            }.AsQueryable();
            _alertRepo.Search(Arg.Any<Expression<Func<Alert, bool>>>()).Returns(alerts);

            var result = _svc.GetAlertsForShipments();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void GetAlertsForInventoryItems_Returns_Alerts_With_InventoryItemId()
        {
            var alerts = new List<Alert>
            {
                new Alert { Id = 1, InventoryItemId = 3, Message = "Low stock" }
            }.AsQueryable();
            _alertRepo.Search(Arg.Any<Expression<Func<Alert, bool>>>()).Returns(alerts);

            var result = _svc.GetAlertsForInventoryItems();

            Assert.Single(result);
        }

        [Fact]
        public void GetInventoryHistoryChanges_Returns_History_For_Date_Range()
        {
            var from = new DateTime(2026, 1, 1);
            var to = new DateTime(2026, 1, 31);

            var inventoryItems = new List<InventoryItem>
            {
                new InventoryItem { Id = 1, StorageFacilityId = 5, ItemId = 10, StockCount = 10 }
            }.AsQueryable();
            _inventoryItemRepo.Search(Arg.Any<Expression<Func<InventoryItem, bool>>>()).Returns(inventoryItems);

            _itemRepo.Find(10).Returns(new Item { Id = 10, Name = "Widget" });

            var history = new List<InventoryHistory>
            {
                new InventoryHistory { Id = 1, InventoryItemId = 1, OldStock = 15, NewStock = 10, ChangedAt = new DateTime(2026, 1, 15) }
            }.AsQueryable();
            _inventoryHistoryRepo.Search(Arg.Any<Expression<Func<InventoryHistory, bool>>>()).Returns(history);

            var result = _svc.GetInventoryHistoryChanges(5, from, to);

            Assert.Single(result);
            Assert.Equal("Widget", result[0].ItemName);
            Assert.Single(result[0].History);
        }
    }
}
