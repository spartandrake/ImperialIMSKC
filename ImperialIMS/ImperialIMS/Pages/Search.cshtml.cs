using ImperialIMS.Models;
using ImperialIMS.Services;
using ImperialIMS.Repos;
using ImperialIMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace ImperialIMS.Pages
{
    public class SearchModel : PageModel
    {
        private readonly InventoryItemService _inventoryItemService;
        private readonly ItemService _itemService;
        private readonly StorageFacilityService _storageFacilityService;
        private readonly CategoryService _categoryService;
        private readonly IRepo<ItemCategory> _itemCategoryRepo;
        private readonly ManifestService _manifestService;
        private readonly ShipmentService _shipmentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public List<SearchResultItem> Results { get; set; } = new();
        public List<StorageFacility> StorageFacilities { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Query { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? FacilityId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }

        [BindProperty]
        public int AddInventoryItemId { get; set; }
        [BindProperty]
        public int AddQuantity { get; set; }

        public SearchModel(
            InventoryItemService inventoryItemService,
            ItemService itemService,
            StorageFacilityService storageFacilityService,
            CategoryService categoryService,
            IRepo<ItemCategory> itemCategoryRepo,
            ManifestService manifestService,
            ShipmentService shipmentService,
            UserManager<ApplicationUser> userManager)
        {
            _inventoryItemService = inventoryItemService;
            _itemService = itemService;
            _storageFacilityService = storageFacilityService;
            _categoryService = categoryService;
            _itemCategoryRepo = itemCategoryRepo;
            _manifestService = manifestService;
            _shipmentService = shipmentService;
            _userManager = userManager;
        }

        public void OnGet(string? query)
        {
            if (!string.IsNullOrWhiteSpace(query))
                Query = query;

            LoadFilterData();
            Results = BuildResults();
        }
        public IActionResult OnPostAddToShipment()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            var inventoryItem = _inventoryItemService.Get(AddInventoryItemId);
            if (inventoryItem.Id == 0 || AddQuantity <= 0)
                return RedirectToPage();

            var userId = _userManager.GetUserId(User);
            //if there is no current pending shipment , create one (singleton)
            var shipment = _shipmentService.CreateShipmentForUser(userId);
            //add manifest entry for this item and quantity to the pending shipment
            var manifest = new Manifest
            {
                InventoryItemId = inventoryItem.Id,
                ShippingId = shipment.Id,
                amount = AddQuantity
            };
            _manifestService.Add(manifest);
            if(Query != null)
            {
                return RedirectToPage(new { query = Query });
            }
            return RedirectToPage();
        }

        private void LoadFilterData()
        {
            StorageFacilities = _storageFacilityService.GetAll();
            Categories = _categoryService.GetAll();
        }

        private List<SearchResultItem> BuildResults()
        {
            var inventoryItems = _inventoryItemService.GetAll();
            var itemsById = _itemService.GetAll().ToDictionary(i => i.Id);
            var facilitiesById = _storageFacilityService.GetAll().ToDictionary(f => f.Id);
            var categoriesById = _categoryService.GetAll().ToDictionary(c => c.Id);
            var itemCategories = _itemCategoryRepo.Search(x => !x.IsDeleted).ToList();

            var results = inventoryItems.Select(inv =>
            {
                itemsById.TryGetValue(inv.ItemId, out var item);
                facilitiesById.TryGetValue(inv.StorageFacilityId, out var facility);
                var catId = itemCategories.FirstOrDefault(ic => ic.ItemId == inv.ItemId)?.CategoryId;
                string catName = catId.HasValue && categoriesById.TryGetValue(catId.Value, out var cat)
                    ? cat.Name
                    : "Uncategorized";
                string status = inv.StockCount == 0 ? "Out of Stock"
                    : inv.StockCount <= inv.ReorderLevel ? "Low Stock"
                    : "In Stock";

                return new SearchResultItem
                {
                    InventoryItemId = inv.Id,
                    ItemId = inv.ItemId,
                    ItemName = item?.Name ?? "(Unknown)",
                    CategoryName = catName,
                    StockCount = inv.StockCount,
                    StorageFacilityId = inv.StorageFacilityId,
                    StorageFacilityName = facility?.Name ?? "(Unknown)",
                    StockStatus = status
                };
            }).ToList();

            if (!string.IsNullOrWhiteSpace(Query))
                results = results.Where(r => r.ItemName.Contains(Query, StringComparison.OrdinalIgnoreCase)).ToList();
            if (FacilityId.HasValue && FacilityId > 0)
                results = results.Where(r => r.StorageFacilityId == FacilityId).ToList();
            if (CategoryId.HasValue && CategoryId > 0)
            {
                var matchingItemIds = itemCategories
                    .Where(ic => ic.CategoryId == CategoryId)
                    .Select(ic => ic.ItemId)
                    .ToHashSet();
                results = results.Where(r => matchingItemIds.Contains(r.ItemId)).ToList();
            }
            if (!string.IsNullOrWhiteSpace(Status))
                results = results.Where(r => r.StockStatus == Status).ToList();

            return results;
        }
    }
}