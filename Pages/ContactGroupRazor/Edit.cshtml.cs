using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactApp.Data;
using ContactApp.Models;

namespace ContactApp.Pages.ContactGroupRazor
{
    public class EditModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;

        public EditModel(ContactApp.Data.ContactContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ContactGroup ContactGroup { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContactGroup = await _context.ContactGroups.FirstOrDefaultAsync(m => m.ContactGroupId == id);

            if (ContactGroup == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ContactGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactGroupExists(ContactGroup.ContactGroupId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ContactGroupExists(long id)
        {
            return _context.ContactGroups.Any(e => e.ContactGroupId == id);
        }
    }
}
