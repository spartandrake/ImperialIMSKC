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
            Claim adminClaim = new Claim(PolicyTypes.Role, PolicyValues.Admin);
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

                    await userManager.AddClaimsAsync(adminUser, [adminClaim]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public static async Task SeedTestUsersAsync(UserManager<ApplicationUser> userManager)
        {
            Claim managerClaim = new Claim(PolicyTypes.Role, PolicyValues.Manager);
            Claim officerClaim = new Claim(PolicyTypes.Role, PolicyValues.Officer);
            Claim auditorClaim = new Claim(PolicyTypes.Role, PolicyValues.Auditor);
            Claim adminClaim = new Claim(PolicyTypes.Role, PolicyValues.Admin);
            try
            {
                if (await userManager.FindByNameAsync("test@imperialims.com") == null)
                {
                    ApplicationUser testUser = new ApplicationUser
                    {
                        UserName = "test@imperialims.com",
                        Email = "test@imperialims.com",
                        EmailConfirmed = true,
                        FirstName = "Test",
                        LastName = "User"
                    };
                    await userManager.CreateAsync(testUser, "Test1234!");
                    await userManager.AddClaimsAsync(testUser, new[] { managerClaim, officerClaim, auditorClaim });
                    Console.WriteLine("Seeded test user: test@imperialims.com");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to seed test user: {ex.Message}");
            }
            try
            {
                if (await userManager.FindByNameAsync("admin@imperialims.com") == null)
                {
                    ApplicationUser adminUser = new ApplicationUser
                    {
                        UserName = "admin@imperialims.com",
                        Email = "admin@imperialims.com",
                        EmailConfirmed = true,
                        FirstName = "Admin",
                        LastName = "User"
                    };
                    await userManager.CreateAsync(adminUser, "Admin1234!");
                    await userManager.AddClaimsAsync(adminUser, new[] { adminClaim });
                    Console.WriteLine("Seeded admin user: admin@imperialims.com");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to seed admin user: {ex.Message}");
            }
        }
    }
}

