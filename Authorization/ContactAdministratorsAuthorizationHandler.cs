using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Authorization
{
	public class ContactAdministratorsAuthorizationHandler
		: AuthorizationHandler<OperationAuthorizationRequirement, Contact>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
		{
			if (context.User ==null)
			{
				return Task.CompletedTask;
			}

			//管理员可以做任何事情。
			if (context.User.IsInRole(ContactOperations.ContactAdministratorsRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
