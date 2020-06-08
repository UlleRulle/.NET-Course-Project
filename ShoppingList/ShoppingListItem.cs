namespace ShoppingList
{
    public class ShoppingListItem
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int ShoppingListID { get; set; }

        public ShoppingListItem(string name, int shoppingListID)
        {
            this.Name = name;
            this.ShoppingListID = shoppingListID;
        }
    }
}
