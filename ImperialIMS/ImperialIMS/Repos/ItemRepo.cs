
using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class ItemRepo : RepoBase<Item>
    {
        public ItemRepo(IConfiguration config) : base(config)
        {
        }
    }
}
