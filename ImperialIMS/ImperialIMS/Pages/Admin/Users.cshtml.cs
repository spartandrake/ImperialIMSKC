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
        public string NewRole { get; set; }
        public List<ApplicationUser> Users { get; set; } 
        public String[] Roles { get; } = new String[] { "Admin", "Manager", "Officer", "Default" };
        public UsersModel(ILogger<UsersModel> logger, ApplicationUserService applicationUserService)
        {
            _logger = logger;
            _applicationUserService = applicationUserService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            _applicationUser = await _applicationUserService.GetUserAsync(Id);
            Users = _applicationUserService.GetAllUsers();
            Users.Remove(_applicationUser);
            return Page();
        }
        public async Task<IActionResult> OnPostChangeRoleAsync() 
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrEmpty(NewRole))
            {
                return RedirectToPage();
            }
            //this feels too tightly coupled, but it works for now. If we add more roles, we'll need to update this switch statement.
            (string Type, string Value) = NewRole switch
            {
                "Admin" => (PolicyTypes.IsAdmin, "true"),
                "Manager" => (PolicyTypes.IsManager, "true"),
                "Officer" => (PolicyTypes.IsOfficer, "true"),
                "Default" => (PolicyTypes.IsDefault, "true"),
                _ => throw new ArgumentException("Invalid role selected.")
            };
            await _applicationUserService.UpsertUserClaimsAsync(Id, Type, Value);
            return RedirectToPage();
        }
    }
}
