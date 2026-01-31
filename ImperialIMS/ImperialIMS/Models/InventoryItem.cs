namespace ImperialIMS.Models
{
    public class InventoryItem : EntityBase
    {
        public Item Item { get; set; }
        public int ItemId { get; set; }
        public StorageFacility StorageFacility { get; set; }
        public int StorageFacilityId { get; set; }
        public int StockCount { get; set; }
        public int MaxStockLevel { get; set; }
        public int ReorderLevel { get; set; }
    }
}
