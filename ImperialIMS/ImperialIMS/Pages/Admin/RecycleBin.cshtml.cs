using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Admin
{
    //[Authorize(Policy = "AdminOnly")]
    public class RecycleBinModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
