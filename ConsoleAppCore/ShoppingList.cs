using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleAppCore
{
    [Table("ShoppingList")]
    public class ShoppingList
    {
        [Key]
        public int ShoppingListID { get; set; }
        [Required]
        [StringLength(50)]
        public string Title { get; set; }
        public ICollection<ShoppingListItem> Items { get; set; }
        public DateTime Date { get; set; }
        public int UserProfileUserID { get; set; }

    }
}
