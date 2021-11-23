using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ContactApp.Data;
using ContactApp.Models;

namespace ContactApp.RazorPages
{
    public class CreateModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;
        private readonly TokenClient _tokenClient;
        private readonly ServiceClient _serviceClient;

        public CreateModel(ContactApp.Data.ContactContext context, TokenClient tokenClient, ServiceClient serviceClient)
        {
            _context = context;
            _serviceClient = serviceClient;
            _tokenClient = tokenClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var headers = await _tokenClient.GetAuthHeaderAsync();
            string relativePath = ContactConstant.GetContact;
            await _serviceClient.PostDataAsync<Contact>(relativePath, headers, Contact);
            
            return RedirectToPage("./Index");
        }
    }
}
