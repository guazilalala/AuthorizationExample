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
    public class DetailsModel : DI_BasePageModel
    {
		public DetailsModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
		{
		}

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
            return Page();
        }

		public async Task<IActionResult> OnPostAsync(int id,ContactStatus contactStatus)
		{
			var contact = await Context.Contacts.FirstOrDefaultAsync(m=>m.ContactId == id);

			if (contact == null)
			{
				return NotFound();
			}

			var contactOperation = (contactStatus == ContactStatus.Approved)
				? ContactOperations.Approve
				: ContactOperations.Reject;

			var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact, contactOperation);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

			contact.Status = contactStatus;

			Context.Update(contact);
			await Context.SaveChangesAsync();

			return RedirectToPage("./Index");
		}
    }
}
