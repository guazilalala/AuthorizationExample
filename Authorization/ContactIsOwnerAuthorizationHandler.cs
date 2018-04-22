using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Authorization
{
	public class ContactIsOwnerAuthorizationHandler
		: AuthorizationHandler<OperationAuthorizationRequirement, Contact>
	{
		UserManager<ApplicationUser> _userManager;

		public ContactIsOwnerAuthorizationHandler(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
		{
			if (context.User == null || resource == null)
			{
				return Task.CompletedTask;
			}

			//如果我们不要求得到CRUD许可，请返回。

			if (requirement.Name != Constants.CreateOperationName && 
				requirement.Name != Constants.ReadOperationName &&
				requirement.Name != Constants.UpdateOperationName &&
				requirement.Name != Constants.DeleteOperationName)
			{
				return Task.CompletedTask;
			}

			if (resource.OwnerID == _userManager.GetUserId(context.User))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
