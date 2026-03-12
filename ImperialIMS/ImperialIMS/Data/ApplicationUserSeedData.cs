using ImperialIMS.Helpers;
using ImperialIMS.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ImperialIMS.Data
{
    public static class ApplicationUserSeedData
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            Claim adminClaim = new Claim(PolicyTypes.IsAdmin, PolicyValues.True);
            Claim moderatorClaim = new Claim(PolicyTypes.IsOfficer, PolicyValues.False);
            String adminUserName = config.GetSection("AdminUser")["Email"] ?? String.Empty;
            String adminPass = config.GetSection("AdminUser")["Password"] ?? String.Empty;
            try
            {
                // Add new user and their claims if they don't exist
                if ((await userManager.FindByNameAsync(adminUserName) == null
                    && !String.IsNullOrEmpty(adminUserName)
                    && !String.IsNullOrEmpty(adminPass)))
                {
                    ApplicationUser adminUser = new ApplicationUser
                    {
                        UserName = adminUserName,
                        Email = adminUserName,
                        EmailConfirmed = true,
                        FirstName = "Admin",
                        LastName = "User"
                    };

                    await userManager.CreateAsync(adminUser, adminPass);

                    adminUser = await userManager.FindByNameAsync(adminUserName);

                    await userManager.AddClaimsAsync(adminUser, [adminClaim, moderatorClaim]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}

