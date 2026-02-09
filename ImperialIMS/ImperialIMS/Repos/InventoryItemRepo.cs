
using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class InventoryItemRepo : RepoBase<InventoryItem>
    {
        public InventoryItemRepo(IConfiguration config) : base(config)
        {
        }
    }
}
