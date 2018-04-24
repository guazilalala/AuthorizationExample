using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AuthorizationExample.Authorization;

namespace AuthorizationExample.Pages.Contacts
{
    public class EditModel : DI_BasePageModel
    {
		public EditModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
		{
			var contactStatus = from ContactStatus s in Enum.GetValues(typeof(ContactStatus))
								select new { Key = (int)s, Value = s.ToString() };

			ContactStatusList = contactStatus.Select(r => new SelectListItem
			{
				Value = r.Key.ToString(),
				Text = r.Value
			});
		}

		[BindProperty]
        public Contact Contact { get; set; }
		[BindProperty]
		public IEnumerable<SelectListItem> ContactStatusList { get; set; }

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

			var isAuthorized = await AuthorizationService.AuthorizeAsync(
													User,Contact, ContactOperations.Update);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
			var contact = await Context.Contacts.AsNoTracking()
				.FirstOrDefaultAsync(m => m.ContactId == id);

			if (contact == null)
			{
				return NotFound();
			}

			var isAuthorized = await AuthorizationService.AuthorizeAsync(
				User, contact, ContactOperations.Update);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

			Contact.OwnerID = contact.OwnerID;

            Context.Attach(Contact).State = EntityState.Modified;

			if (contact.Status == ContactStatus.Approved)
			{
				var canApprove = await AuthorizationService.AuthorizeAsync(User, contact, ContactOperations.Approve);

				if (!canApprove.Succeeded)
				{
					contact.Status = ContactStatus.Submitted;
				}
			}

            try
            {
                await Context.SaveChangesAsync();
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

        private bool ContactExists(int id)
        {
            return Context.Contacts.Any(e => e.ContactId == id);
        }
    }
}
