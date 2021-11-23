using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ContactApp.Data;
using ContactApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ContactApp.ContactRazor
{
    public class IndexModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;
        private readonly TokenClient _tokenClient;
        private readonly ServiceClient _serviceClient;
        public IndexModel(ContactApp.Data.ContactContext context , TokenClient tokenClient, ServiceClient serviceClient)
        {
            _context = context;
            _serviceClient = serviceClient;
            _tokenClient = tokenClient;
        }

        public IList<Contact> Contact { get;set; }
        public Contact ContactList { get; set; }

        public async Task OnGetAsync()
        {
            var headers = await _tokenClient.GetAuthHeaderAsync();
            string relativePath = ContactConstant.GetContact;
            Contact = await _serviceClient.GetDataAsync<IList<Contact>>(relativePath, headers);
        }
    }
}
