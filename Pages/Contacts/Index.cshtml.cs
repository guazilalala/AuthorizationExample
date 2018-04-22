using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AuthorizationExample.Data;

namespace AuthorizationExample.Pages.Contacts
{
    public class IndexModel : PageModel
    {
        private readonly AuthorizationExample.Data.ApplicationDbContext _context;

        public IndexModel(AuthorizationExample.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Contact> Contact { get;set; }

        public async Task OnGetAsync()
        {
            Contact = await _context.Contacts.ToListAsync();
        }
    }
}
