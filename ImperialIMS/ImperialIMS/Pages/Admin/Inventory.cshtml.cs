using ImperialIMS.Models;
using ImperialIMS.Repos;
using ImperialIMS.Services;
using ImperialIMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace ImperialIMS.Pages.Admin
{
    //[Authorize(Policy = "AdminOnly")]
    public class InventoryModel : PageModel
    {
        public List<StorageFacility> StorageFacilities { get; set; } = new List<StorageFacility>();
        public List<Item> Items { get; set; } = new List<Item>();
        private ItemService _itemService { get; set; }
        private StorageFacilityService _storageService { get; set; }
        private InventoryItemService _inventoryService { get; set; }
        private ILogger<InventoryModel> _log { get; set; }
        [BindProperty]
        public int selectedItemId { get; set; }
        [BindProperty]
        public int selectedStorageFacilityId { get; set; }
        [BindProperty]
        public int InventoryQuantity { get; set; }
        [BindProperty]
        public int MaxStockLevel { get; set; }
        [BindProperty]
        public int ReorderLevel { get; set; }
        [BindProperty]
        public int EditId { get; set; }
        public List<(InventoryItem inv, string itemName, string facilityName)> AllInventory { get; set; } = new();
        public InventoryModel(ItemService itemService, StorageFacilityService storageFacilityService, InventoryItemService inventoryItemService, ILogger<InventoryModel> logger)
        {
            _itemService = itemService;
            _storageService = storageFacilityService;
            _inventoryService = inventoryItemService;
            _log = logger;
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id != null) 
            { 
                EditId = (int)id;
                Items = new List<Item> { _itemService.Get(_inventoryService.Get((int)id).ItemId) };
                StorageFacilities = new List<StorageFacility> { _storageService.Get(_inventoryService.Get((int)id).StorageFacilityId) };
            }
            else 
            {
                var allInv = _inventoryService.GetAll();
                AllInventory = allInv.Select(i => (
                    i,
                    _itemService.Get(i.ItemId)?.Name ?? "Unknown",
                    _storageService.Get(i.StorageFacilityId)?.Name ?? "Unknown"
                )).ToList();
                Items = _itemService.GetAll();
                StorageFacilities = _storageService.GetAll();
            }
            
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _log.LogInformation("Selected Item Id: {itemId}, Selected Storage Facility Id: {storageId}, Inventory Quantity: {quantity}", selectedItemId, selectedStorageFacilityId, InventoryQuantity);
                    if (EditId != 0)
                    {
                        InventoryItem inventoryItem = _inventoryService.Get(EditId);
                        inventoryItem.ItemId = selectedItemId;
                        inventoryItem.StorageFacilityId = selectedStorageFacilityId;
                        inventoryItem.StockCount = InventoryQuantity;
                        inventoryItem.MaxStockLevel = MaxStockLevel;
                        inventoryItem.ReorderLevel = ReorderLevel;
                        _inventoryService.Update(inventoryItem);
                        _log.LogInformation("Updated inventory item with Id: {id} to Item Id: {itemId}, Storage Facility Id: {storageId} with quantity: {quantity}", EditId, selectedItemId, selectedStorageFacilityId, InventoryQuantity);
                    }
                    else if (selectedItemId != 0 && selectedStorageFacilityId != 0)
                    {
                        InventoryItem inventoryItem = new InventoryItem
                        {
                            ItemId = selectedItemId,
                            StorageFacilityId = selectedStorageFacilityId,
                            StockCount = InventoryQuantity,
                            MaxStockLevel = MaxStockLevel,
                            ReorderLevel = ReorderLevel
                        };
                        _inventoryService.Add(inventoryItem);
                        _log.LogInformation("Added inventory item with Item Id: {itemId} to Storage Facility Id: {storageId} with quantity: {quantity}", selectedItemId, selectedStorageFacilityId, InventoryQuantity);
                    }
                    else
                    {
                        _log.LogWarning("Selected Item Id or Storage Facility Id is zero. Cannot add inventory item.");
                    }
                }
                else
                {
                    _log.LogError("Model State is invalid.");
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error posting inventory update." + ex.Message);
            }
            return RedirectToPage();
        }
        public IActionResult OnPostDelete(int id)
        {
            _inventoryService.Delete(id);
            return RedirectToPage();
        }
    }
}
