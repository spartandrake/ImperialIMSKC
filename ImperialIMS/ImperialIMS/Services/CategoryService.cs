
using ImperialIMS.Models;

namespace ImperialIMS.Services
{
    public class CategoryService : ServiceBase<Category>
    {
        public CategoryService(Repos.IRepo<Category> repo, IConfiguration configuration, ILogger<Category> logger) : base(repo, configuration, logger)
        {
        }
    }
}
