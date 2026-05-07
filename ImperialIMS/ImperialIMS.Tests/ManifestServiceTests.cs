using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ImperialIMS.Tests
{
    public class ManifestServiceTests
    {
        private readonly IRepo<Manifest> _repo;
        private readonly ManifestService _svc;

        public ManifestServiceTests()
        {
            _repo = Substitute.For<IRepo<Manifest>>();
            var logger = Substitute.For<ILogger<Manifest>>();
            var config = new ConfigurationBuilder().Build();
            _svc = new ManifestService(_repo, config, logger);
        }

        [Fact]
        public void GetAllForShipment_Returns_Manifests_For_Shipment()
        {
            _repo.Search().Returns(new List<Manifest>
            {
                new Manifest { Id = 1, ShippingId = 10, InventoryItemId = 1, amount = 5 },
                new Manifest { Id = 2, ShippingId = 10, InventoryItemId = 2, amount = 3 },
                new Manifest { Id = 3, ShippingId = 99, InventoryItemId = 3, amount = 1 }
            }.AsQueryable());

            var result = _svc.GetAllForShipment(10);

            Assert.Equal(2, result.Count);
            Assert.All(result, m => Assert.Equal(10, m.ShippingId));
        }

        [Fact]
        public void GetAllForShipment_Returns_Empty_When_No_Manifests_For_Shipment()
        {
            _repo.Search().Returns(new List<Manifest>
            {
                new Manifest { Id = 1, ShippingId = 99, InventoryItemId = 1, amount = 5 }
            }.AsQueryable());

            var result = _svc.GetAllForShipment(10);

            Assert.Empty(result);
        }
    }
}
