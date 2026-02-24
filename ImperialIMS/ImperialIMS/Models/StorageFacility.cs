namespace ImperialIMS.Models
{
    public class StorageFacility : EntityBase
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public List<InventoryItem> Inventory { get; set; }
    }
}
