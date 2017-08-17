using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Query my roles request
	/// </summary>
	public class QueryMyRolesRequest
	{
		/// <summary>
		/// Session that will be used in this request
		/// </summary>
		[Required]
		public string Session { get; set; }

		/// <summary>
		/// ThingID that which roles are got.
		/// </summary>
		[Required]
		[ThingId]
		public string ThingId { get; set; }

	}
	/// <summary>
	/// Query My Roles Response
	/// </summary>
	public class QueryMyRolesResponse
	{
		/// <summary>
		/// List of Roles.
		/// </summary>
		public List<string> Roles { get; set; }
	}
}
