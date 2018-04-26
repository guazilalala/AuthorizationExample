using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationExample.Data
{
	public class Contact
	{
		public int ContactId { get; set; }

		// user ID from AspNetUser table.
		public string OwnerID { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		public ContactStatus Status { get; set; }
	}

	public enum ContactStatus
	{
		/// <summary>
		/// 提交
		/// </summary>
		Submitted,
		/// <summary>
		/// 批准
		/// </summary>
		Approved,
		/// <summary>
		/// 拒绝
		/// </summary>
		Rejected
	}
}
