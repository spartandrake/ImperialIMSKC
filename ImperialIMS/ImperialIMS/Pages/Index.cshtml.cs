using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ImperialIMS.Pages
{
     
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ShipmentService _shipmentService;
        private readonly AlertService _alertService;
        private readonly HttpContextAccessor _httpContextAccessor;
        public string CurrentUserId { get; set; }
        public List<Shipment> Shipments { get; set; } = new List<Shipment>();
        public List<Alert> Alerts { get; set; } = new List<Alert>();
        public IndexModel(ShipmentService shipmentService, ILogger<IndexModel> logger, AlertService alertService)
        {
            _shipmentService = shipmentService;
            _alertService = alertService;
            _logger = logger;
        }
        public void OnGet()
        {
            if (User.Identity.IsAuthenticated)
            {
                try
                {
                    var claimsIdentity = (ClaimsIdentity)User.Identity;
                    var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        CurrentUserId = claim.Value;
                    }
                    Shipments = _shipmentService.GetAllForUser(CurrentUserId);
                    Alerts = _alertService.GetAllForUser(CurrentUserId);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
