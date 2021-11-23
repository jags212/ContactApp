using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactApp.Data;
using ContactApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ContactApp.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class ContactGroupsController : ControllerBase
    {
        private readonly ContactContext _context;

        public ContactGroupsController(ContactContext context)
        {
            _context = context;
        }

        // GET: api/ContactGroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ContactGroup>>> GetContactGroups()
        {
            return await _context.ContactGroups
                                 .Include(o => o.ContactItems)                                                                         
                                 .ToListAsync();
        }

        // GET: api/ContactGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactGroup>> GetContactGroup(long id)
        {
            var contactGroup = await _context.ContactGroups
                                             .Include(o => o.ContactItems)
                                             .Where(s => s.ContactGroupId == id)
                                             .FirstOrDefaultAsync();

            if (contactGroup == null)
            {
                return NotFound();
            }

            return contactGroup;
        }

        // PUT: api/ContactGroups/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactGroup(long id, ContactGroup contactGroup)
        {
            if (id != contactGroup.ContactGroupId)
            {
                return BadRequest();
            }

            _context.Entry(contactGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactGroupExists(id))
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

        // POST: api/ContactGroups
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ContactGroup>> PostContactGroup(ContactGroup contactGroup)
        {
            _context.ContactGroups.Add(contactGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactGroup", new { id = contactGroup.ContactGroupId }, contactGroup);
        }

        // DELETE: api/ContactGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactGroup>> DeleteContactGroup(long id)
        {
            var contactGroup = await _context.ContactGroups.FindAsync(id);
            if (contactGroup == null)
            {
                return NotFound();
            }

            _context.ContactGroups.Remove(contactGroup);
            await _context.SaveChangesAsync();

            return contactGroup;
        }

        private bool ContactGroupExists(long id)
        {
            return _context.ContactGroups.Any(e => e.ContactGroupId == id);
        }
    }
}
