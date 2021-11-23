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
    public class IndexModel : PageModel
    {
        private readonly ContactApp.Data.ContactContext _context;

        public IndexModel(ContactApp.Data.ContactContext context)
        {
            _context = context;
        }

        public IList<ContactGroup> ContactGroup { get;set; }

        public async Task OnGetAsync()
        {
            ContactGroup = await _context.ContactGroups.ToListAsync();
        }
    }
}
