
using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class ShipmentRepo : RepoBase<Shipment>
    {
        public ShipmentRepo(IConfiguration config) : base(config)
        {
        }
    }
}
