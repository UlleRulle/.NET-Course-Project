using Microsoft.EntityFrameworkCore;

namespace ConsoleAppCore
{
    public class ShoppingListContext : DbContext
    {
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

        public ShoppingListContext(DbContextOptions<ShoppingListContext> options) : base(options) { }

    }
}
