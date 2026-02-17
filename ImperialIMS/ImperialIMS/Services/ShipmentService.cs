
using ImperialIMS.Models;

namespace ImperialIMS.Services
{
    public class ShipmentService : ServiceBase<Shipment>
    {
        public ShipmentService(Repos.IRepo<Shipment> repo, IConfiguration configuration, ILogger<Shipment> logger) : base(repo, configuration, logger)
        {
        }
    }
}
