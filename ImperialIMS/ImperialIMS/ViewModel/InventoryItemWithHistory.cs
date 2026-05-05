using ImperialIMS.Models;

namespace ImperialIMS.ViewModel
{
    public class InventoryItemWithHistory
    {
        public InventoryItem InventoryItem { get; set; }
        public string ItemName { get; set; }
        public IEnumerable<InventoryHistory> History { get; set; }
    }
}
