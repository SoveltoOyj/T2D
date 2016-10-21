using System;
using System.Collections.Generic;
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
		/// Session ID
		/// </summary>
		public string Session { get; set; }
		/// <summary>
		/// Thing ID: CreatorFQDN/UniqueString
		/// </summary>
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
