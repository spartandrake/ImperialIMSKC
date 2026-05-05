using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.Models
{
    public class InventoryHistory : EntityBase
    {
        public int InventoryItemId { get; set; }
        [ForeignKey("InventoryItemId")]
        public InventoryItem InventoryItem { get; set; }
        public int OldStock {  get; set; }
        public int NewStock { get; set; }
        public string ChangeReason { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
