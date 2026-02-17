using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class ItemService : ServiceBase<Item>
    {
        public ItemService(IRepo<Item> repo, IConfiguration configuration, ILogger<Item> logger) : base(repo, configuration, logger)
        {
        }
    }
}
