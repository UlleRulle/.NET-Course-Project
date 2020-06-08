using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleAppCore
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }

        public ICollection<ShoppingList> ShoppingLists { get; set; }

    }
}
