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
    public class ShoppingListsController : ControllerBase
    {
        private readonly ShoppingListContext _context;

        public ShoppingListsController(ShoppingListContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingList>>> GetShoppingLists()
        {
            var lists = await _context.ShoppingLists.ToListAsync();
            return lists;
        }

        // GET: api/ShoppingLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingList>> GetShoppingList(int id)
        {
            var shoppingList = await _context.ShoppingLists.FindAsync(id);

            if (shoppingList == null)
            {
                return NotFound();
            }

            return shoppingList;
        }

        /// <summary>
        /// Gets list of ShoppingList by UserProfile's UserID
        /// </summary>
        /// <param name="userId">The ID of the UserProfile</param>
        // GET: api/ShoppingLists/user/{userid}
        [HttpGet("userid")]
        [Route("user/{userid}")]
        public async Task<ActionResult<List<ShoppingList>>> GetShoppingListsByUserId(int userId)
        {
            var shoppingLists = await _context.ShoppingLists.Where(t => t.UserProfileUserID.Equals(userId)).ToListAsync();

            return shoppingLists;
        }

        /// <summary>
        /// Checks if API can connect to the database
        /// </summary>
        // PING
        [HttpGet("ping")]
        [Route("ping")]
        public bool GetPingResult()
        {
            return _context.Database.CanConnect();
        }

        // PUT: api/ShoppingLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingList(int id, ShoppingList shoppingList)
        {
            if (id != shoppingList.ShoppingListID)
            {
                return BadRequest();
            }

            _context.Entry(shoppingList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingListExists(id))
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

        // POST: api/ShoppingLists
        [HttpPost]
        public async Task<ActionResult<ShoppingList>> PostShoppingList(ShoppingList shoppingList)
        {
            _context.ShoppingLists.Add(shoppingList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingList", new { id = shoppingList.ShoppingListID }, shoppingList);
        }

        // DELETE: api/ShoppingLists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ShoppingList>> DeleteShoppingList(int id)
        {
            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();

            return shoppingList;
        }

        private bool ShoppingListExists(int id)
        {
            return _context.ShoppingLists.Any(e => e.ShoppingListID == id);
        }
    }
}
