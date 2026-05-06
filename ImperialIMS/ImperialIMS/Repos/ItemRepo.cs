
using ImperialIMS.Data;
using ImperialIMS.Models;
using Microsoft.EntityFrameworkCore;

namespace ImperialIMS.Repos
{
    public class ItemRepo : RepoBase<Item>
    {
        public ItemRepo(IConfiguration config) : base(config)
        {
        }
        public ItemRepo(DbContextOptions<ApplicationDbContext> options, IConfiguration config, ApplicationDbContext db)
            : base(options, config, db) { }
    }
}
