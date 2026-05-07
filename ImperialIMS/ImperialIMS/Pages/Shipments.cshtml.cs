using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages
{
    public class ShipmentsModel : PageModel
    {
        private readonly ILogger<ShipmentsModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ShipmentService _shipmentService;
        public List<Shipment> userShipments { get; set; }
        public ShipmentsModel(ILogger<ShipmentsModel> logger, ShipmentService shipmentService, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _shipmentService = shipmentService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            userShipments = _shipmentService.GetAllForUser(userId);
            return Page();
        }
    }
}