using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContactApp.Data;
using ContactApp.Models;

namespace ContactApp.Pages.ContactGroupRazor
{
    public class CreateModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;

        public CreateModel(ContactApp.Data.ContactContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ContactGroup ContactGroup { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ContactGroups.Add(ContactGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
