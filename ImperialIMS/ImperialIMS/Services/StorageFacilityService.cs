using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class StorageFacilityService : ServiceBase<StorageFacility>
    {
        public StorageFacilityService(IRepo<StorageFacility> repo, IConfiguration configuration, ILogger<StorageFacility> logger) : base(repo, configuration, logger)
        {
        }
    }
}
