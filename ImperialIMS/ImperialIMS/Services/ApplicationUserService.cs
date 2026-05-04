using ImperialIMS.Helpers;
using ImperialIMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ImperialIMS.Services
{
    public class ApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUserService> _logger;
        private ApplicationUser user;
        private List<Claim> claims;
        public ApplicationUserService(UserManager<ApplicationUser> userManager, ILogger<ApplicationUserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
            user = new ApplicationUser();
            claims = new List<Claim>();
        }
        public async Task<ApplicationUser> GetUserAsync(String UserId)
        {
            return await _userManager.FindByIdAsync(UserId);
        }
        public List<ApplicationUser> GetAllUsers()
        {
            return _userManager.Users.ToList<ApplicationUser>();
        }
        public async Task<bool> IsAdminAsync(String UserId)
        {
            var role = await GetUserRoleAsync(UserId);
            return role.Equals(PolicyValues.Admin);
        }
        public async Task<String> GetUserRoleAsync(String UserId)
        {
            var claims = await GetApplicationClaimsAsync(UserId);
            return claims.Where(c => c.Type.Equals(PolicyTypes.Role)).Select(c => c.Value).FirstOrDefault() ?? PolicyValues.Default;
        }
        public async Task<List<Claim>> GetApplicationClaimsAsync(String UserId)
        {
            if (!user.Id.Equals(UserId))
            {
                user = await GetUserAsync(UserId);
            }
            claims = (List<Claim>)await _userManager.GetClaimsAsync(user);
            return claims;
        }
        public async Task SetUserRoleAsync(string UserId, string Role)
        {
            if (!user.Id.Equals(UserId))
                user = await GetUserAsync(UserId);

            var existing = (await GetApplicationClaimsAsync(UserId))
                .FirstOrDefault(c => c.Type == PolicyTypes.Role);

            if (existing != null)
                await _userManager.RemoveClaimAsync(user, existing);

            var result = await _userManager.AddClaimAsync(user, new Claim(PolicyTypes.Role, Role));
            if (result.Succeeded)
                _logger.LogInformation("Set Role={Role} for user {UserId}", Role, UserId);
            else
                _logger.LogError("Failed to set Role={Role} for user {UserId}", Role, UserId);
        }
        public async Task InvalidateUserSessionAsync(string UserId)
        {
            var targetUser = await GetUserAsync(UserId);
            if (targetUser != null)
                await _userManager.UpdateSecurityStampAsync(targetUser);
        }
    }
}
