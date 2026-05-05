using ImperialIMS.Models;

namespace ImperialIMS.ViewModel
{
    public class InventoryItemWithHistory
    {
        public InventoryItemWithHistory InventoryItem { get; set; }
        public string ItemName { get; set; }
        public IEnumerable<InventoryHistory> History { get; set; }
    }
}
