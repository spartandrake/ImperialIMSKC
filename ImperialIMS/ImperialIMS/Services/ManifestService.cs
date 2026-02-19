using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ManifestService : ServiceBase<Manifest>
    {
        public ManifestService(IRepo<Manifest> repo, IConfiguration configuration, ILogger<Manifest> logger) : base(repo, configuration, logger)
        {
        }
    }
}
