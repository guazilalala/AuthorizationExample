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
		private readonly IModelMetadataProvider _modelMetadataProvider;
		public CreateModel(ApplicationDbContext context, IAuthorizationService authorizationService, UserManager<ApplicationUser> userManager, IModelMetadataProvider modelMetadataProvider) : base(context, authorizationService, userManager)
		{
			_modelMetadataProvider = modelMetadataProvider;
			var sontactStatus = from ContactStatus s in Enum.GetValues(typeof(ContactStatus))
						   select new { ID = (int)s, Name = s.ToString() };

			ContactStatusList = sontactStatus.ToDictionary(keySelector=>keySelector.ID,KeyValuePair=>KeyValuePair.Name);
		}

		public IActionResult OnGet()
        {
			return Page();
        }

        [BindProperty]
        public Contact Contact { get; set; }

		[BindProperty]
		public Dictionary<int,string> ContactStatusList { get; set; }

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

		public IEnumerable<SelectListItem> GetEnumSelectList<TEnum>() where TEnum : struct
		{
			var type = typeof(TEnum);
			var metadata = _modelMetadataProvider.GetMetadataForType(type);
			if (!metadata.IsEnum || metadata.IsFlagsEnum)
			{
			}

			return metadata;
		}
	}
}