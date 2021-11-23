using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactApp.Data;
using ContactApp.Models;

namespace ContactApp.Pages.ContactGroupRazor
{
    public class DeleteModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;

        public DeleteModel(ContactApp.Data.ContactContext context)
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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ContactGroup = await _context.ContactGroups.FindAsync(id);

            if (ContactGroup != null)
            {
                _context.ContactGroups.Remove(ContactGroup);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
