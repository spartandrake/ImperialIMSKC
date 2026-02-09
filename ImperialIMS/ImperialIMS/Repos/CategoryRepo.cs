
using ImperialIMS.Models;

namespace ImperialIMS.Repos
{
    public class CategoryRepo : RepoBase<Category>
    {
        public CategoryRepo(IConfiguration config) : base(config)
        {
        }
    }
}
