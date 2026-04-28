using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly ILogger<UsersModel> _logger;
        
        private readonly ApplicationUserService _applicationUserService;
        private ApplicationUser _applicationUser { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public List<ApplicationUser> Users { get; set; } 
        public String[] Roles { get; } = new String[] { "Admin", "Manager", "Auditor", "Default" };
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
    }
}
