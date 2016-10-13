using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{
	public class QueryMyRolesRequest
	{
		public string Session { get; set; }
		public string ThingId { get; set; }
	}
	public class QueryMyRolesResponse
	{
		public List<string> Roles { get; set; }
	}
}
