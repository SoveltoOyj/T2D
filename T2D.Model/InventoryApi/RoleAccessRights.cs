using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Set Role Access Rights.
	/// To remove Rights, set empty Rights to the AttributeRole.
	/// </summary>
	public class SetRoleAccessRightsRequest : BaseRequest
	{
		/// <summary>
		/// The role for which access rights are set.
		/// </summary>
		[Required]
		public string RoleForRights { get; set; }
		[Required]
		public List<AttributeRoleRight> AttributeRoleRights { get; set; }
	}

	/// <summary>
	/// Get Role Access Rights.
	/// </summary>
	public class GetRoleAccessRightsRequest : BaseRequest
	{
		/// <summary>
		/// The role for which access rights are got.
		/// </summary>
		[Required]
		public string RoleForRights { get; set; }
	}

	/// <summary>
	/// Get Role Access Rights.
	/// </summary>
	public class GetRoleAccessRightsResponse 
	{
		/// <summary>
		/// All attributes that have Role Rights.
		/// </summary>
		public List<AttributeRoleRight> AttributeRoleRights { get; set; }
	}


	public class AttributeRoleRight
	{
		/// <summary>
		/// Attribute (enum type Attribute).
		/// </summary>
		[Required]
		public string Attribute { get; set; }
		/// <summary>
		/// Array of rights (enum type Right)
		/// </summary>
		public List<string> RoleAccessRights { get; set; }
	}
}
