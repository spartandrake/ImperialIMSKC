using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class RecycleBinModel : PageModel
    {
        private ItemService _itemService;
        private StorageFacilityService _sfService;
        private CategoryService _catService;
        private ILogger<RecycleBinModel> _log;
        public List<Item> DeletedItems { get; set; }
        public List<StorageFacility> DeletedFacilities { get; set; }
        public List<Category> DeletedCategories { get; set; }
        public RecycleBinModel(ItemService itemService, StorageFacilityService sfService, CategoryService catService, ILogger<RecycleBinModel> log  )
        {
            _itemService = itemService;
            _sfService = sfService;
            _catService = catService;
            _log = log;
        }
        public void OnGet()
        {
            DeletedItems = _itemService.GetRecycleBin();
            DeletedFacilities = _sfService.GetRecycleBin();
            DeletedCategories = _catService.GetRecycleBin();
        }
        public async Task<IActionResult> OnPostRestore(int id) 
        { 
            _itemService.UnDelete(id);
            _log.LogInformation($"Restored item with id {id} from recycle bin.");
            return RedirectToPage("/Admin/RecycleBin"); 
        }
        public async Task<IActionResult> OnPostPermanentDelete(int id)
        {
            _itemService.Remove(id);
            _log.LogInformation($"Permanently deleted item with id {id} from recycle bin.");
            return RedirectToPage("/Admin/RecycleBin");
        }
    }
}
