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
			if (requirement.Name != Constants.ApproveOperationName&&
				requirement.Name != Constants.RejectOperationName)
			{
				return Task.CompletedTask;
			}

			if (context.User.IsInRole(Constants.ContactManagersRole))
			{
				context.Succeed(requirement);
			}

			return Task.CompletedTask;
		}
	}
}
