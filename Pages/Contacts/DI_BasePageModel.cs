using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Pages.Contacts
{
    public class DI_BasePageModel:PageModel
    {
		protected ApplicationDbContext Context { get; }
		protected IAuthorizationService AuthorizationService { get; }
		protected UserManager<ApplicationUser> UserManager { get; }
		public DI_BasePageModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager)
		{
			Context = context;
			AuthorizationService = authorizationService;
			UserManager = userManager;
		}

    }
}
