using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class ItemCategoryRepo : RepoBase<ItemCategory>
    {
        public ItemCategoryRepo(IConfiguration config) : base(config)
        {
        }
    }
}
