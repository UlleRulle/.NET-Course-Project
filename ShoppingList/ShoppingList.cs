using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShoppingList
{
    public class ShoppingList
    {
        public int ShoppingListID { get; set; }
        [MaxLength]
        public string Title { get; set; }
        public ICollection<ShoppingListItem> Items { get; set; }
        public DateTime Date { get; set; }
        public int UserProfileUserID { get; set; }

        public ShoppingList(string title, DateTime date, int userProfileUserID)
        {
            this.Title = title;
            this.Date = date;
            this.UserProfileUserID = userProfileUserID;
        }
    }
}
