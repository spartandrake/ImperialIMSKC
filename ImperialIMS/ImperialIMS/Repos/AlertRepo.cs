
using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class AlertRepo : RepoBase<Alert>
    {
        public AlertRepo(IConfiguration config) : base(config)
        {
        }
    }
}
