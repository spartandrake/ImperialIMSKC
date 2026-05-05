using ImperialIMS.Models;
using ImperialIMS.Services;
using ImperialIMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;
using System.Security.Claims;

namespace ImperialIMS.Pages.Admin
{
    //[Authorize(Policy = "AdminOnly")]
    public class ItemModel : PageModel
    {
        private ItemService _service { get; set; }
        private ILogger<ItemModel> _log { get; set; }
        [BindProperty]
        public ViewItem Item { get; set; }
        public Item NewItem { get; set; }
        public List<Item> AllItems { get; set; }

        public ItemModel(ItemService service, ILogger<ItemModel> log)
        {
            _service = service;
            _log = log;
            Item = new ViewItem();
            NewItem = new Item();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            AllItems = _service.GetAll();
            try
            {
                if (id != null)
                {
                    NewItem = _service.Get((int)id);
                    Item.Id = NewItem.Id;
                    Item.Name = NewItem.Name;
                    Item.Description = NewItem.Description;
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error loading Item CRUD page. " + ex.Message);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostCreateOrUpdate()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (Item.Id != 0)
                    {
                        //authorize?
                        NewItem = _service.Get(Item.Id);
                        NewItem.Name = Item.Name;
                        NewItem.Description = Item.Description;
                        _service.Update(NewItem);
                        _log.LogInformation("Updated Item with id of {id}", NewItem.Id);
                    }
                    if (Item.Id == 0)
                    {
                        NewItem = new Item();
                        NewItem.Name = Item.Name;
                        NewItem.Description = Item.Description;
                        _service.Add(NewItem);
                        _log.LogInformation("Created new Item with id of {id}", NewItem.Id);
                    }
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
            return LocalRedirect("/Search/" + NewItem.Name);
        }
        public IActionResult OnPostDelete(int id)
        {
            _service.Delete(id);
            return RedirectToPage();
        }
    }
}
