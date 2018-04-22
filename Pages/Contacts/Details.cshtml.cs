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
    public class DetailsModel : PageModel
    {
        private readonly AuthorizationExample.Data.ApplicationDbContext _context;

        public DetailsModel(AuthorizationExample.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await _context.Contacts.SingleOrDefaultAsync(m => m.ContactId == id);

            if (Contact == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
