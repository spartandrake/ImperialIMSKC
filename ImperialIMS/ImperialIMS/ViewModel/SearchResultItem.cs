namespace ImperialIMS.ViewModel
{
    public class SearchResultItem
    {
        public int InventoryItemId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public int StockCount { get; set; }
        public int StorageFacilityId { get; set; }
        public string StorageFacilityName { get; set; }
        public string StockStatus { get; set; }
    }
}