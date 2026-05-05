using ImperialIMS.Models;
using ImperialIMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ImperialIMS.Pages.Shipments
{
    public class DetailsModel : PageModel
    {
        public Shipment shipment { get; set; }
        public List<Manifest> manifests { get; set; }
        [BindProperty]
        public int ManifestId { get; set; }
        [BindProperty]
        public int Quantity { get; set; }
        [BindProperty]
        public int ShippingId { get; set; }
        private readonly ShipmentService _shipmentService;
        private readonly ManifestService _manifestService;
        private readonly Random _trackingIdGenerator = new Random();

        public DetailsModel(ShipmentService shipmentService, ManifestService manifestService)
        {
            _shipmentService = shipmentService;
            _manifestService = manifestService;
        }

        public IActionResult OnGet(int ShipmentId)
        {
            shipment = _shipmentService.Get(ShipmentId);
            manifests = _manifestService.GetAllForShipment(ShipmentId);
            return Page();
        }

        public IActionResult OnPostConfirmShipment(int ShipmentId)
        {
            _shipmentService.MarkShipmentAsInTransit(ShipmentId, _trackingIdGenerator.Next());
            _shipmentService.UpdateInventory(ShipmentId);
            return RedirectToPage("/Shipments");
        }

        public IActionResult OnPostConfirmReceipt(int ShipmentId)
        {
            _shipmentService.MarkShipmentAsReceived(ShipmentId);
            return RedirectToPage("/Shipments");
        }

        public IActionResult OnPostCancelShipment(int ShipmentId)
        {
            _shipmentService.MarkShipmentAsCancelled(ShipmentId);
            return RedirectToPage("/Shipments");
        }

        public IActionResult OnPostModifyShipment(int ShipmentId)
        {
            return RedirectToPage("/Shipments/Details", new { ShipmentId });
        }
        public IActionResult OnPostModifyManifest(int ManifestId, int Quantity)
        {
            var manifest = _manifestService.Get(ManifestId);
            manifest.amount = Quantity;
            _manifestService.Update(manifest);
            return RedirectToPage(new {ShipmentId = manifest.ShippingId});
        }
    }
}
