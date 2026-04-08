using ImperialIMS.Models;
using ImperialIMS.Pages.Admin;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages
{
    public class ShipmentsModel : PageModel
    {
        private readonly ILogger<ShipmentsModel> _logger;

        private readonly ApplicationUserService _applicationUserService;
        private readonly ShipmentService _shipmentService;
        private ApplicationUser _applicationUser { get; set; }
        public List<Shipment> userShipments { get; set; }
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            _applicationUser = await _applicationUserService.GetUserAsync(Id);
            userShipments = _shipmentService.GetAllForUser(Id);
            return Page();
        }
    }
}
