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
    public class UserProfilesController : ControllerBase
    {
        private readonly ShoppingListContext _context;

        public UserProfilesController(ShoppingListContext context)
        {
            _context = context;
        }

        // GET: api/UserProfiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetUserProfiles()
        {
            return await _context.UserProfiles.ToListAsync();
        }

        // GET: api/UserProfiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return userProfile;
        }

        /// <summary>
        /// Gets UserProfile by username
        /// </summary>
        /// <param name="username">The username to the UserProfile</param>
        // GET: api/UserProfiles/user/{username}
        [HttpGet("username")]
        [Route("user/{username}")]
        public async Task<ActionResult<UserProfile>> GetUserProfileByUsername(string username)
        {
            var userProfile = await _context.UserProfiles.Where(t => t.UserName.Equals(username)).FirstOrDefaultAsync();

            return userProfile;
        }

        // PUT: api/UserProfiles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserProfile(int id, UserProfile userProfile)
        {
            if (id != userProfile.UserID)
            {
                return BadRequest();
            }

            _context.Entry(userProfile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserProfileExists(id))
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

        // POST: api/UserProfiles
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostUserProfile(UserProfile userProfile)
        {
            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserProfile", new { id = userProfile.UserID }, userProfile);
        }

        // DELETE: api/UserProfiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserProfile>> DeleteUserProfile(int id)
        {
            var userProfile = await _context.UserProfiles.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return userProfile;
        }

        private bool UserProfileExists(int id)
        {
            return _context.UserProfiles.Any(e => e.UserID == id);
        }
    }
}
