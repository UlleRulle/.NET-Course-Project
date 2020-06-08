using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleAppCore
{
    [Table("ShoppingListItem")]
    public class ShoppingListItem
    {
        [Key]
        public int ItemID { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public int ShoppingListID { get; set; }

    }
}
