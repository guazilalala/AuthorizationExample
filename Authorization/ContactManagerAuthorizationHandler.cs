using AuthorizationExample.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Authorization
{
	public class ContactManagerAuthorizationHandler
		: AuthorizationHandler<OperationAuthorizationRequirement, Contact>
	{
		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, Contact resource)
		{
			if (context.User == null|| resource == null)
			{
				return Task.CompletedTask;
			}

			//如果不请求批准/拒绝，返回。
			if (requirement.Name != ContactOperations.ApproveOperationName&&
				requirement.Name != ContactOperations.RejectOperationName)
			{
				return Task.CompletedTask;
			}

			if (context.User.IsInRole(ContactOperations.ContactManagersRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
