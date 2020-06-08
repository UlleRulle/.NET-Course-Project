using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConsoleAppCore;

namespace DonauAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListItemsController : ControllerBase
    {
        private readonly ShoppingListContext _context;

        public ShoppingListItemsController(ShoppingListContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingListItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingListItem>>> GetShoppingListItems()
        {
            var items = await _context.ShoppingListItems.ToListAsync();
            return items;
        }

        // GET: api/ShoppingListItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingListItem>> GetShoppingListItem(int id)
        {
            var shoppingListItem = await _context.ShoppingListItems.FindAsync(id);

            if (shoppingListItem == null)
            {
                return NotFound();
            }

            return shoppingListItem;
        }

        /// <summary>
        /// Gets list of ShoppingListItem by ShoopingListID
        /// </summary>
        /// <param name="shoppingListId">The ID of the ShoppingList</param>
        // GET: api/ShoppingListItems/ShoppingList/{shoppinglistid}
        [HttpGet("shoppinglistid")]
        [Route("ShoppingList/{shoppinglistid}")]
        public async Task<ActionResult<List<ShoppingListItem>>> GetShoppingListItemByShoppingListId(int shoppingListId)
        {
            var shoppingListItems = await _context.ShoppingListItems.Where(t => t.ShoppingListID.Equals(shoppingListId)).ToListAsync();

            return shoppingListItems;
        }

        // PUT: api/ShoppingListItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingListItem(int id, ShoppingListItem shoppingListItem)
        {
            if (id != shoppingListItem.ItemID)
            {
                return BadRequest();
            }

            _context.Entry(shoppingListItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingListItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ShoppingListItems
        [HttpPost]
        public async Task<ActionResult<ShoppingListItem>> PostShoppingListItem(ShoppingListItem shoppingListItem)
        {
            _context.ShoppingListItems.Add(shoppingListItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingListItem", new { id = shoppingListItem.ItemID }, shoppingListItem);
        }

        /// <summary>
        /// Post a list of ShoppingListItem
        /// </summary>
        /// <param name="shoppingListItemList">List of ShoppingListItem</param>
        // POST: api/ShoppingListItems
        [HttpPost("batch")]
        public async Task<ActionResult<List<ShoppingListItem>>> PostShoppingListItems([FromBody] List<ShoppingListItem> shoppingListItemList)
        {
            foreach(var item in shoppingListItemList)
            {
                _context.ShoppingListItems.Add(item);
            }
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetShoppingListItems", shoppingListItemList);
        }

        // DELETE: api/ShoppingListItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ShoppingListItem>> DeleteShoppingListItem(int id)
        {
            var shoppingListItem = await _context.ShoppingListItems.FindAsync(id);
            if (shoppingListItem == null)
            {
                return NotFound();
            }

            _context.ShoppingListItems.Remove(shoppingListItem);
            await _context.SaveChangesAsync();

            return shoppingListItem;
        }

        /// <summary>
        /// Deletes all items to a ShoppingList by ShoppingListID
        /// </summary>
        /// <param name="id">The ID of a ShoppingList</param>
        // DELETE: api/ShoppingList/id/ShoppingListItems
        [HttpDelete("ShoppingList/{id}")]
        public async Task<ActionResult<List<ShoppingListItem>>> DeleteShoppingListItems(int id)
        {
            List<ShoppingListItem> shoppingListItemList = await _context.ShoppingListItems.Where(e => e.ShoppingListID == id).ToListAsync();
            if (shoppingListItemList == null)
            {
                return NotFound();
            }
            foreach(var shoppingListItem in shoppingListItemList)
            {
                _context.ShoppingListItems.Remove(shoppingListItem);
            }
            await _context.SaveChangesAsync();
            return shoppingListItemList;
        }

        private bool ShoppingListItemExists(int id)
        {
            return _context.ShoppingListItems.Any(e => e.ItemID == id);
        }
    }
}
