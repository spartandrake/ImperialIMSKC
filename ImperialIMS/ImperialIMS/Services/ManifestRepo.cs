using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ManifestRepo : ServiceBase<Manifest>
    {
        public ManifestRepo(IRepo<Manifest> repo, IConfiguration configuration, ILogger<Manifest> logger) : base(repo, configuration, logger)
        {
        }
    }
}
