using ImperialIMS.Models;
using ImperialIMS.Repos;

namespace ImperialIMS.Services
{
    public class AlertService: ServiceBase<Alert>
    {
        private readonly ILogger<Alert> _logger;
        private readonly IRepo<Alert> _repo;
        private IConfiguration _configuration { get; set; }
        public AlertService(Repos.IRepo<Alert> repo, IConfiguration configuration, ILogger<Alert> logger) : base(repo, configuration, logger)
        {
            _logger = logger;
            _repo = repo;
            _configuration = configuration;
        }
        public List<Alert> GetAllForUser(string userid)
        {
            return _repo.Search().Where(s => s.ApplicationUserId.ToString() == userid).ToList();
        }
    }
}
