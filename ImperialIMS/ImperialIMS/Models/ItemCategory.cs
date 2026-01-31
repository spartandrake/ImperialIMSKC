namespace ImperialIMS.Models
{
    public class ItemCategory : EntityBase
    {
        public Item item { get; set; }
        public int itemId { get; set; }
        public Category category { get; set; }
        public int categoryId { get; set; }
    }
}
