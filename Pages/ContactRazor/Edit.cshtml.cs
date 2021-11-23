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

namespace ContactApp.RazorPages
{
    public class EditModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;
        private readonly TokenClient _tokenClient;
        private readonly ServiceClient _serviceClient;

        public EditModel(ContactApp.Data.ContactContext context,TokenClient tokenClient, ServiceClient serviceClient)
        {
            _context = context;
            _serviceClient = serviceClient;
            _tokenClient = tokenClient;
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var headers = await _tokenClient.GetAuthHeaderAsync();
            string relativePath = string.Format(ContactConstant.GetContactById, id);
            Contact = await _serviceClient.GetDataAsync<Contact>(relativePath, headers);

            if (Contact == null)
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

            var headers = await _tokenClient.GetAuthHeaderAsync();
            string relativePath = string.Format(ContactConstant.GetContactById, Contact.ContactId);
            await _serviceClient.PutDataAsync<Contact>(relativePath, headers, Contact);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactExists(Contact.ContactId))
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

        private bool ContactExists(long id)
        {
            return _context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
