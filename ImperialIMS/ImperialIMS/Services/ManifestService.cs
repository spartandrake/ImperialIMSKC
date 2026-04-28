using ImperialIMS.Models;
using ImperialIMS.Repos;
using Microsoft.EntityFrameworkCore;

namespace ImperialIMS.Services
{
    public class ManifestService : ServiceBase<Manifest>
    {
        private readonly IRepo<Manifest> _repo;
        public ManifestService(IRepo<Manifest> repo, IConfiguration configuration, ILogger<Manifest> logger) : base(repo, configuration, logger)
        {
            _repo = repo;
        }
        public List<Manifest> GetAllForShipment(int shipmentId)
        {
            return _repo.Search().Where(m => m.ShippingId == shipmentId)
                .Include(m => m.InventoryItem)
                    .ThenInclude(i => i.Item)
                .Include(m => m.InventoryItem)
                    .ThenInclude(i => i.StorageFacility)
                .ToList();
        }
    }
}
