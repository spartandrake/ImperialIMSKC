using ImperialIMS.Data;
using ImperialIMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ImperialIMS.Repos
{
    public class InventoryHistoryRepo : RepoBase<InventoryHistory>
    {
        public InventoryHistoryRepo(DbContextOptions<ApplicationDbContext> options, IConfiguration config, ApplicationDbContext db) : base(options, config, db)
        {
        }
    }
}
