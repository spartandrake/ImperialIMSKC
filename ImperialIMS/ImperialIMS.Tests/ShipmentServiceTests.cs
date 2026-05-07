using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Linq.Expressions;

namespace ImperialIMS.Tests
{
    public class ShipmentServiceTests
    {
        private readonly IRepo<Shipment> _repo;
        private readonly IRepo<Manifest> _manifestRepo;
        private readonly IRepo<InventoryItem> _inventoryRepo;
        private readonly IRepo<InventoryHistory> _historyRepo;
        private readonly ShipmentService _svc;

        public ShipmentServiceTests()
        {
            _repo = Substitute.For<IRepo<Shipment>>();
            _manifestRepo = Substitute.For<IRepo<Manifest>>();
            _inventoryRepo = Substitute.For<IRepo<InventoryItem>>();
            _historyRepo = Substitute.For<IRepo<InventoryHistory>>();

            var config = new ConfigurationBuilder().Build();
            var shipmentLogger = Substitute.For<ILogger<Shipment>>();
            var manifestLogger = Substitute.For<ILogger<Manifest>>();
            var inventoryLogger = Substitute.For<ILogger<InventoryItem>>();

            var manifestService = new ManifestService(_manifestRepo, config, manifestLogger);
            var inventoryService = new InventoryItemService(_inventoryRepo, _historyRepo, config, inventoryLogger);

            _svc = new ShipmentService(_repo, config, shipmentLogger, manifestService, inventoryService);
        }

        private void SetupShipmentSearch(IEnumerable<Shipment> shipments)
        {
            _repo.Search(Arg.Any<Expression<Func<Shipment, bool>>>()).Returns(shipments.AsQueryable());
        }

        // --- GetAllForUser ---

        [Fact]
        public void GetAllForUser_Returns_Only_Shipments_For_That_User()
        {
            _repo.Search().Returns(new List<Shipment>
            {
                new Shipment { Id = 1, ApplicationUserId = "user-1", Status = ShippingStatus.Pending },
                new Shipment { Id = 2, ApplicationUserId = "user-2", Status = ShippingStatus.Delivered },
                new Shipment { Id = 3, ApplicationUserId = "user-1", Status = ShippingStatus.InTransit }
            }.AsQueryable());

            var result = _svc.GetAllForUser("user-1");

            Assert.Equal(2, result.Count);
            Assert.All(result, s => Assert.Equal("user-1", s.ApplicationUserId));
        }

        // --- CreateShipmentForUser ---

        [Fact]
        public void CreateShipmentForUser_Returns_Existing_Pending_Shipment()
        {
            var existing = new Shipment { Id = 5, ApplicationUserId = "user-1", Status = ShippingStatus.Pending };
            SetupShipmentSearch(new[] { existing });

            var result = _svc.CreateShipmentForUser("user-1");

            Assert.Equal(5, result.Id);
            _repo.DidNotReceive().Add(Arg.Any<Shipment>());
        }

        [Fact]
        public void CreateShipmentForUser_Creates_New_Shipment_When_None_Pending()
        {
            SetupShipmentSearch(Enumerable.Empty<Shipment>());

            var result = _svc.CreateShipmentForUser("user-1");

            _repo.Received(1).Add(Arg.Is<Shipment>(s =>
                s.ApplicationUserId == "user-1" && s.Status == ShippingStatus.Pending));
        }

        // --- MarkShipmentAsReceived ---

        [Fact]
        public void MarkShipmentAsReceived_Sets_Status_To_Delivered()
        {
            var shipment = new Shipment { Id = 1, ApplicationUserId = "user-1", Status = ShippingStatus.InTransit };
            SetupShipmentSearch(new[] { shipment });

            _svc.MarkShipmentAsReceived(1);

            _repo.Received(1).Update(Arg.Is<Shipment>(s => s.Status == ShippingStatus.Delivered));
        }

        [Fact]
        public void MarkShipmentAsReceived_Does_Nothing_When_Shipment_Not_Found()
        {
            SetupShipmentSearch(Enumerable.Empty<Shipment>());

            _svc.MarkShipmentAsReceived(99);

            _repo.DidNotReceive().Update(Arg.Any<Shipment>());
        }

        // --- MarkShipmentAsInTransit ---

        [Fact]
        public void MarkShipmentAsInTransit_Sets_Status_And_TrackingId()
        {
            var shipment = new Shipment { Id = 1, Status = ShippingStatus.Pending };
            SetupShipmentSearch(new[] { shipment });

            _svc.MarkShipmentAsInTransit(1, trackingId: 42);

            _repo.Received(1).Update(Arg.Is<Shipment>(s =>
                s.Status == ShippingStatus.InTransit && s.TrackingId == 42));
        }

        [Fact]
        public void MarkShipmentAsInTransit_Does_Nothing_When_Shipment_Not_Found()
        {
            SetupShipmentSearch(Enumerable.Empty<Shipment>());

            _svc.MarkShipmentAsInTransit(99, 0);

            _repo.DidNotReceive().Update(Arg.Any<Shipment>());
        }

        // --- MarkShipmentAsCancelled ---

        [Fact]
        public void MarkShipmentAsCancelled_Sets_Status_To_Cancelled()
        {
            var shipment = new Shipment { Id = 1, Status = ShippingStatus.Pending };
            SetupShipmentSearch(new[] { shipment });

            _svc.MarkShipmentAsCancelled(1);

            _repo.Received(1).Update(Arg.Is<Shipment>(s => s.Status == ShippingStatus.Cancelled));
        }

        [Fact]
        public void MarkShipmentAsCancelled_Does_Nothing_When_Shipment_Not_Found()
        {
            SetupShipmentSearch(Enumerable.Empty<Shipment>());

            _svc.MarkShipmentAsCancelled(99);

            _repo.DidNotReceive().Update(Arg.Any<Shipment>());
        }

        // --- MarkShipmentAsLost ---

        [Fact]
        public void MarkShipmentAsLost_Sets_Status_To_Lost()
        {
            var shipment = new Shipment { Id = 1, Status = ShippingStatus.InTransit };
            SetupShipmentSearch(new[] { shipment });

            _svc.MarkShipmentAsLost(1);

            _repo.Received(1).Update(Arg.Is<Shipment>(s => s.Status == ShippingStatus.Lost));
        }

        [Fact]
        public void MarkShipmentAsLost_Does_Nothing_When_Shipment_Not_Found()
        {
            SetupShipmentSearch(Enumerable.Empty<Shipment>());

            _svc.MarkShipmentAsLost(99);

            _repo.DidNotReceive().Update(Arg.Any<Shipment>());
        }

        // --- UpdateInventory ---

        [Fact]
        public void UpdateInventory_Decrements_Stock_For_Each_Manifest_Item()
        {
            _manifestRepo.Search().Returns(new List<Manifest>
            {
                new Manifest { Id = 1, ShippingId = 10, InventoryItemId = 1, amount = 5 }
            }.AsQueryable());

            _inventoryRepo.Find(1).Returns(new InventoryItem
            {
                Id = 1,
                StockCount = 20,
                MaxStockLevel = 100,
                ReorderLevel = 5,
                Item = new Item { Id = 1, Name = "Widget" }
            });

            _svc.UpdateInventory(10);

            _inventoryRepo.Received(1).Update(Arg.Is<InventoryItem>(i => i.StockCount == 15));
        }
    }
}
