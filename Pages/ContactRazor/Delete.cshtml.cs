using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactApp.Data;
using ContactApp.Models;

namespace ContactApp.RazorPages
{
    public class DeleteModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;
        private readonly TokenClient _tokenClient;
        private readonly ServiceClient _serviceClient;

        public DeleteModel(ContactApp.Data.ContactContext context,TokenClient tokenClient, ServiceClient serviceClient)
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
            var relativePath = string.Format(ContactConstant.GetContactById, id);

            Contact = await _serviceClient.GetDataAsync<Contact>(relativePath, headers);

            if (Contact == null)
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

            Contact = await _context.Contacts.FindAsync(id);

            if (Contact != null)
            {
                var headers = await _tokenClient.GetAuthHeaderAsync();
                string relativePath = string.Format(ContactConstant.GetContactById, id);
                await _serviceClient.DeleteDataAsync<Contact>(relativePath, headers);
            }

            return RedirectToPage("./Index");
        }
    }
}
