using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Set Role Access Rights
	/// </summary>
	public class SetRoleAccessRighsRequest : BaseRequest
	{
		[Required]
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
		public string[] RoleAccessRights { get; set; }
	}
}
