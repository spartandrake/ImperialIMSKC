using ImperialIMS.Helpers;
using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Security.Claims;

namespace ImperialIMS.Tests
{
    public class ApplicationUserServiceTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationUserService _svc;

        private static ApplicationUser MakeUser(string id = "user-1") =>
            new ApplicationUser { Id = id, FirstName = "Test", LastName = "User", Email = "test@test.com" };

        public ApplicationUserServiceTests()
        {
            var store = Substitute.For<IUserStore<ApplicationUser>>();
            _userManager = Substitute.For<UserManager<ApplicationUser>>(
                store, null, null, null, null, null, null, null, null);
            var logger = Substitute.For<ILogger<ApplicationUserService>>();
            _svc = new ApplicationUserService(_userManager, logger);
        }

        [Fact]
        public async Task GetUserAsync_Returns_User_When_Found()
        {
            var user = MakeUser();
            _userManager.FindByIdAsync("user-1").Returns(user);

            var result = await _svc.GetUserAsync("user-1");

            Assert.Equal("user-1", result.Id);
        }

        [Fact]
        public void GetAllUsers_Returns_All_Users()
        {
            var users = new List<ApplicationUser> { MakeUser("user-1"), MakeUser("user-2") };
            _userManager.Users.Returns(users.AsQueryable());

            var result = _svc.GetAllUsers();

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetApplicationClaimsAsync_Returns_Claims_For_User()
        {
            var user = MakeUser();
            var claims = new List<Claim> { new Claim(PolicyTypes.Role, PolicyValues.Admin) };
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.GetClaimsAsync(user).Returns(Task.FromResult<IList<Claim>>(claims));

            var result = await _svc.GetApplicationClaimsAsync("user-1");

            Assert.Single(result);
            Assert.Equal(PolicyTypes.Role, result[0].Type);
        }

        [Fact]
        public async Task GetUserRoleAsync_Returns_Role_From_Claims()
        {
            var user = MakeUser();
            var claims = new List<Claim> { new Claim(PolicyTypes.Role, PolicyValues.Admin) };
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.GetClaimsAsync(user).Returns(Task.FromResult<IList<Claim>>(claims));

            var result = await _svc.GetUserRoleAsync("user-1");

            Assert.Equal(PolicyValues.Admin, result);
        }

        [Fact]
        public async Task GetUserRoleAsync_Returns_Default_When_No_Role_Claim()
        {
            var user = MakeUser();
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.GetClaimsAsync(user).Returns(Task.FromResult<IList<Claim>>(new List<Claim>()));

            var result = await _svc.GetUserRoleAsync("user-1");

            Assert.Equal(PolicyValues.Default, result);
        }

        [Fact]
        public async Task IsAdminAsync_Returns_True_When_Role_Is_Admin()
        {
            var user = MakeUser();
            var claims = new List<Claim> { new Claim(PolicyTypes.Role, PolicyValues.Admin) };
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.GetClaimsAsync(user).Returns(Task.FromResult<IList<Claim>>(claims));

            var result = await _svc.IsAdminAsync("user-1");

            Assert.True(result);
        }

        [Fact]
        public async Task IsAdminAsync_Returns_False_When_Role_Is_Not_Admin()
        {
            var user = MakeUser();
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.GetClaimsAsync(user).Returns(Task.FromResult<IList<Claim>>(new List<Claim>()));

            var result = await _svc.IsAdminAsync("user-1");

            Assert.False(result);
        }

        [Fact]
        public async Task SetUserRoleAsync_Removes_Existing_Role_And_Adds_New_One()
        {
            var user = MakeUser();
            var existingClaim = new Claim(PolicyTypes.Role, PolicyValues.Default);
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.GetClaimsAsync(user).Returns(Task.FromResult<IList<Claim>>(new List<Claim> { existingClaim }));
            _userManager.RemoveClaimAsync(user, existingClaim).Returns(IdentityResult.Success);
            _userManager.AddClaimAsync(user, Arg.Any<Claim>()).Returns(IdentityResult.Success);

            await _svc.SetUserRoleAsync("user-1", PolicyValues.Admin);

            await _userManager.Received(1).RemoveClaimAsync(user, existingClaim);
            await _userManager.Received(1).AddClaimAsync(user, Arg.Is<Claim>(c =>
                c.Type == PolicyTypes.Role && c.Value == PolicyValues.Admin));
        }

        [Fact]
        public async Task InvalidateUserSessionAsync_Calls_UpdateSecurityStamp()
        {
            var user = MakeUser();
            _userManager.FindByIdAsync("user-1").Returns(user);
            _userManager.UpdateSecurityStampAsync(user).Returns(IdentityResult.Success);

            await _svc.InvalidateUserSessionAsync("user-1");

            await _userManager.Received(1).UpdateSecurityStampAsync(user);
        }
    }
}
