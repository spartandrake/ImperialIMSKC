using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.Models
{
    public class InventoryItem : EntityBase
    {
        public Item Item { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public StorageFacility StorageFacility { get; set; }
        [ForeignKey("StorageFacility")]
        public int StorageFacilityId { get; set; }
        public int StockCount { get; set; }
        public int MaxStockLevel { get; set; }
        public int ReorderLevel { get; set; }
    }
}
