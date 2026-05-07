using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ImperialIMS.Tests
{
    public class AlertServiceTests
    {
        private readonly IRepo<Alert> _repo;
        private readonly AlertService _svc;

        public AlertServiceTests()
        {
            _repo = Substitute.For<IRepo<Alert>>();
            var logger = Substitute.For<ILogger<Alert>>();
            var config = new ConfigurationBuilder().Build();
            _svc = new AlertService(_repo, config, logger);
        }

        [Fact]
        public void GetAllForUser_Returns_Only_Alerts_For_That_User()
        {
            _repo.Search().Returns(new List<Alert>
            {
                new Alert { Id = 1, Message = "Low stock", ApplicationUserId = "user-1" },
                new Alert { Id = 2, Message = "Delay",     ApplicationUserId = "user-2" },
                new Alert { Id = 3, Message = "Low stock", ApplicationUserId = "user-1" }
            }.AsQueryable());

            var result = _svc.GetAllForUser("user-1");

            Assert.Equal(2, result.Count);
            Assert.All(result, a => Assert.Equal("user-1", a.ApplicationUserId));
        }

        [Fact]
        public void GetAllForUser_Returns_Empty_When_No_Alerts_For_User()
        {
            _repo.Search().Returns(new List<Alert>
            {
                new Alert { Id = 1, Message = "Low stock", ApplicationUserId = "user-2" }
            }.AsQueryable());

            var result = _svc.GetAllForUser("user-1");

            Assert.Empty(result);
        }
    }
}
