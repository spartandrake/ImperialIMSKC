using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class ManifestRepo : RepoBase<Manifest>
    {
        public ManifestRepo(IConfiguration config) : base(config)
        {
        }
    }
}
