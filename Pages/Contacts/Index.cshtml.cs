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
    public class IndexModel : DI_BasePageModel
    {
		public IndexModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager) : base(context, authorizationService, userManager)
		{
		}

		public IList<Contact> Contact { get;set; }

        public async Task OnGetAsync()
        {
			var contacts = from c in Context.Contacts
						   select c;

			var isAuthorized = User.IsInRole(Constants.ContactManagersRole) ||
							   User.IsInRole(Constants.ContactAdministratorsRole);

			var currentUserId = UserManager.GetUserId(User);

			if (!isAuthorized)
			{
				contacts = contacts.Where(c=>c.Status == ContactStatus.Approved
				||c.OwnerID == currentUserId);
			}

            Contact = await contacts.ToListAsync();
        }
    }
}
