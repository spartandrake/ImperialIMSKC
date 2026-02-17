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
            if (!user.Id.Equals(UserId))
            {
                user = await GetUserAsync(UserId);
            }
            foreach (var claim in await GetApplicationClaimsAsync(UserId))
            {
                if (claim.Type.Equals(PolicyTypes.IsAdmin))
                {
                    return Boolean.Parse(claim.Value);
                }
            }
            return false;
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
        public async Task UpsertUserClaimsAsync(String UserId, String Type, String Value)
        {
            Claim? Claim;
            try
            {
                if (Type.Equals(PolicyTypes.IsAdmin) || Type.Equals(PolicyTypes.IsManager))
                {
                    Claim = new Claim(Type, Value);
                }
                else throw new InvalidOperationException("This is an invalid claim for this application.");
                if (UserId != null || UserId != "" && Claim != null)
                {
                    if (!user.Id.Equals(UserId))
                    {
                        user = await GetUserAsync(UserId);
                    }
                    foreach (Claim claim in await GetApplicationClaimsAsync(UserId))
                    {
                        if (claim.Type.Equals(Claim.Type))
                        {
                            await _userManager.RemoveClaimAsync(user, claim);
                            break;
                        }
                    }
                    IdentityResult result = await _userManager.AddClaimAsync(user, Claim);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Added Claim " + Claim.Type + " with value " + Claim.Value + " to user " + UserId);
                    }
                    else
                    {
                        _logger.LogError("Failed to add Claim " + Claim.Type + " with value " + Claim.Value + " to user " + UserId);
                        throw new InvalidOperationException("Failed to add Claim " + Claim.Type + " with value " + Claim.Value + " to user " + UserId);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
