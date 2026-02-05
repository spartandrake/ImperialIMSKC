using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.Models
{
    public class ItemCategory : EntityBase
    {
        public Item Item { get; set; }
        [ForeignKey("Item")]
        public int ItemId { get; set; }
        public Category Category { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
    }
}
