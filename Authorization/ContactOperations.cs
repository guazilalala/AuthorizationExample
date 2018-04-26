﻿using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Authorization
{
	public static class ContactOperations
	{
		public static OperationAuthorizationRequirement Create =
			new OperationAuthorizationRequirement { Name = Constants.CreateOperationName };
		public static OperationAuthorizationRequirement Read =
			new OperationAuthorizationRequirement { Name = Constants.ReadOperationName };
		public static OperationAuthorizationRequirement Update =
			new OperationAuthorizationRequirement { Name = Constants.UpdateOperationName };
		public static OperationAuthorizationRequirement Delete =
			new OperationAuthorizationRequirement { Name = Constants.DeleteOperationName };
		public static OperationAuthorizationRequirement Approve =
			new OperationAuthorizationRequirement { Name = Constants.ApproveOperationName };
		public static OperationAuthorizationRequirement Reject =
			new OperationAuthorizationRequirement { Name = Constants.RejectOperationName };
	}

	public class Constants
	{
		public const string CreateOperationName = "Create";
		public const string ReadOperationName = "Read";
		public const string UpdateOperationName = "Update";
		public const string DeleteOperationName = "Delete";
		public const string ApproveOperationName = "Approve";
		public const string RejectOperationName = "Reject";
		public const string ContactAdministratorsRole = "ContactAdministrators";
		public const string ContactManagersRole = "ContactManagers";
	}

}
