using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ImperialIMS.ViewModel
{
    public class ViewStorageFacility
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(512)]
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
    }
}
