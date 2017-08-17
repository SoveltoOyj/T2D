using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Set Role MemberList.
	/// </summary>
	public class SetRoleMemberListRequest : BaseRequest
	{
		/// <summary>
		/// The role to which Memberlist will be set.
		/// </summary>
		[Required]
		public string RoleForMemberList { get; set; }

		/// <summary>
		/// ThingIds that are Rolemembers.
		/// </summary>
		[Required]
		public List<string> MemberThingIds { get; set; }
	}

	/// <summary>
	/// Get Role MemberList.
	/// </summary>
	public class GetRoleMemberListRequest : BaseRequest
	{
		/// <summary>
		/// The role for which access rights are got.
		/// </summary>
		[Required]
		public string RoleForMemberList { get; set; }
	}

	/// <summary>
	/// Get Role Access Rights.
	/// </summary>
	public class GetRoleMemberListResponse
	{
		/// <summary>
		/// All ThingIds that are in requested role.
		/// </summary>
		public List<string> ThingIds { get; set; }
	}

}
