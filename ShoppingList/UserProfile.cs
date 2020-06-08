using System.Collections.Generic;

namespace ShoppingList
{
    public class UserProfile
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public ICollection<ShoppingList> ShoppingLists { get; set; }

    }
}
