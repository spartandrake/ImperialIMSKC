using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using ImperialIMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace ImperialIMS.Pages.Admin
{
    public class InventoryModel : PageModel
    {
        public List<StorageFacility> StorageFacilities { get; set; } = new List<StorageFacility>();
        public List<Item> Items { get; set; } = new List<Item>();
        private ItemService _itemService { get; set; }
        private StorageFacilityService _storageService { get; set; }
        private ILogger<InventoryModel> _log { get; set; }
        [BindProperty]
        public int selectedItemId { get; set; }
        [BindProperty]
        public int selectedStorageFacilityId { get; set; }
        [BindProperty]
        public int InventoryQuantity { get; set; }
        public InventoryModel(ItemService itemService, StorageFacilityService storageFacilityService, ILogger<InventoryModel> logger)
        {
            _itemService = itemService;
            _storageService = storageFacilityService;
            _log = logger;
        }
        public void OnGet()
        {
            Items = _itemService.GetAll();
            StorageFacilities = _storageService.GetAll();
        }
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                }
                else
                {
                    _log.LogError("Model State is invalid.");
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error posting Discussion Thread." + ex.Message);
            }
            return Page();
        }
    }
}
