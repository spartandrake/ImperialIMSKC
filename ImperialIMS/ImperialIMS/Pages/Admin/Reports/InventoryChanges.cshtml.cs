using ImperialIMS.Models;
using ImperialIMS.Services;
using ImperialIMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Admin.Reports
{
    [Authorize(Policy = "AdminOnly")]
    public class InventoryChangesModel : PageModel
    {
        private readonly ReportService _reportService;
        private readonly StorageFacilityService _facilityService;

        public List<StorageFacility> StorageFacilities { get; set; } = new();
        public List<InventoryItemWithHistory> ItemsWithHistory { get; set; } = new();
        [BindProperty(SupportsGet = true)]
        public int? SelectedFacilityId { get; set; }

        [BindProperty(SupportsGet = true)] 
        public DateTime? From { get; set; }
        [BindProperty(SupportsGet = true)] 
        public DateTime? To { get; set; }

        public InventoryChangesModel(ReportService reportService, StorageFacilityService facilityService)
        {
            _reportService = reportService;
            _facilityService = facilityService;
        }

        public void OnGet()
        {
            StorageFacilities = _facilityService.GetAll();
            if (SelectedFacilityId.HasValue && From.HasValue && To.HasValue)
            {
                ItemsWithHistory = _reportService.GetInventoryHistoryChanges(SelectedFacilityId.Value, From.Value, To.Value);
            }
        }
    }
}
