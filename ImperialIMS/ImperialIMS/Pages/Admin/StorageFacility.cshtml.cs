using ImperialIMS.Models;
using ImperialIMS.Services;
using ImperialIMS.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class StorageFacilityModel : PageModel
    {
        private StorageFacilityService _service { get; set; }
        private ILogger<StorageFacilityModel> _log { get; set; }
        [BindProperty]
        public ViewStorageFacility StorageFacility { get; set; }
        public StorageFacility NewStorageFacility { get; set; }
        public List<StorageFacility> AllStorageFacilities { get; set; }

        public StorageFacilityModel(StorageFacilityService service, ILogger<StorageFacilityModel> log)
        {
            _service = service;
            _log = log;
            StorageFacility = new ViewStorageFacility();
            NewStorageFacility = new StorageFacility();
        }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            AllStorageFacilities = _service.GetAll();
            try
            {
                if (id != null)
                {
                    NewStorageFacility = _service.Get((int)id);
                    StorageFacility.Id = NewStorageFacility.Id;
                    StorageFacility.Name = NewStorageFacility.Name;
                    StorageFacility.Location = NewStorageFacility.Location;
                }
            }
            catch (Exception ex)
            {
                _log.LogError("Error loading StorageFacility CRUD page. " + ex.Message);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostCreateOrUpdate()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (StorageFacility.Id != 0)
                    {
                        //authorize?
                        NewStorageFacility = _service.Get(StorageFacility.Id);
                        NewStorageFacility.Name = StorageFacility.Name;
                        NewStorageFacility.Location = StorageFacility.Location;
                        _service.Update(NewStorageFacility);
                        _log.LogInformation("Updated StorageFacility with id of {id}", NewStorageFacility.Id);
                    }
                    if (StorageFacility.Id == 0)
                    {
                        NewStorageFacility = new StorageFacility();
                        NewStorageFacility.Name = StorageFacility.Name;
                        NewStorageFacility.Location = StorageFacility.Location;
                        _service.Add(NewStorageFacility);
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
            return LocalRedirect("/Search/" + NewStorageFacility.Name);
        }
        public IActionResult OnPostDelete(int id)
        {
            _service.Delete(id);
            return RedirectToPage();
        }
    }
}

