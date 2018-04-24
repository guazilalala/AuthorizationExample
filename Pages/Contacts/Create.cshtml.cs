using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AuthorizationExample.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AuthorizationExample.Pages.Contacts
{
    public class CreateModel : DI_BasePageModel
    {
		public CreateModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
		{
			var contactStatus = from ContactStatus s in Enum.GetValues(typeof(ContactStatus))
						   select new { Key = (int)s, Value = s.ToString() };

			ContactStatusList = contactStatus.Select(r => new SelectListItem
			{
				Value = r.Key.ToString(),
				Text = r.Value
			});

		}

		public IActionResult OnGet()
        {
			return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

		[BindProperty]
		public IEnumerable<SelectListItem> ContactStatusList { get; set; }

		public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			Contact.OwnerID = UserManager.GetUserId(User);

			var isAuthorized = await AuthorizationService.AuthorizeAsync(
				User, Contact, ContactOperations.Create);

			if (!isAuthorized.Succeeded)
			{
				return new ChallengeResult();
			}

            Context.Contacts.Add(Contact);

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
	}
}