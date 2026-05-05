using ImperialIMS.Helpers;
using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Admin
{
    //[Authorize(Policy = "AdminOnly")]
    public class UsersModel : PageModel
    {
        private readonly ILogger<UsersModel> _logger;

        private readonly ApplicationUserService _applicationUserService;
        private ApplicationUser _applicationUser { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        [BindProperty]
        public string NewRole { get; set; }
        public List<ApplicationUser> Users { get; set; }
        public string[] Roles { get; } = [PolicyValues.Admin, PolicyValues.Manager, PolicyValues.Auditor, PolicyValues.Default];
        public Dictionary<string, string> UserRoles { get; set; } = new();

        public UsersModel(ILogger<UsersModel> logger, ApplicationUserService applicationUserService)
        {
            _logger = logger;
            _applicationUserService = applicationUserService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            _applicationUser = await _applicationUserService.GetUserAsync(UserId);
            Users = _applicationUserService.GetAllUsers();
            Users.Remove(_applicationUser);

            foreach (var user in Users)
                UserRoles[user.Id] = await _applicationUserService.GetUserRoleAsync(user.Id);

            return Page();
        }

        public async Task<IActionResult> OnPostChangeRoleAsync()
        {
            if (string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(NewRole))
                return RedirectToPage();

            await _applicationUserService.SetUserRoleAsync(UserId, NewRole);
            await _applicationUserService.InvalidateUserSessionAsync(UserId);
            return RedirectToPage();
        }
    }
}
