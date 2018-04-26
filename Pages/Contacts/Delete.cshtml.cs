using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AuthorizationExample.Authorization;

namespace AuthorizationExample.Pages.Contacts
{
    public class DeleteModel : DI_BasePageModel
    {
		public DeleteModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
		{
		}

		[BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await Context.Contacts.SingleOrDefaultAsync(m => m.ContactId == id);

            if (Contact == null)
            {
                return NotFound();
            }

			var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Delete);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Contact = await Context.Contacts.FindAsync(id);

            if (Contact == null)
            {
				return NotFound();
            }

			var isAuthorized = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Delete);


			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

			Context.Contacts.Remove(Contact);
			await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
