using ImperialIMS.Models;
using ImperialIMS.Repos;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Core;

namespace ImperialIMS.Services
{
    public class ItemService : ServiceBase<Item>
    {
        private readonly ILogger<Item> _logger;
        private readonly IRepo<Item> _repo;
        public ItemService(IRepo<Item> repo, IConfiguration configuration, ILogger<Item> logger) : base(repo, configuration, logger)
        {
            _repo = repo;
            _logger = logger;
        }
        public List<Item> Search(string searchString)
        {
            try
            {
                return _repo.Search()
                    .Include(i => i.Name)
                    .Include(i => i.Description)
                    .Where(i => i.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase) 
                    || (i.Description != null && i.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase))).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for items with search string: {SearchString}", searchString);
                return new List<Item>();
            }
        }
    }
}
