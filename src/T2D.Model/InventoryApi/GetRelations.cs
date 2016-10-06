using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{

	public abstract class GetAttributeRelationsBaseRequest
	{
		public string Session { get; set; }
		public ThingId ThingId { get; set; }
		public string Role { get; set; }
	}
	public class GetRelationsRequest : GetAttributeRelationsBaseRequest
	{
	}
	public class GetRelationsResponse
	{
		public List<RoleThingsClass> RoleThings { get; set; }

		public class RoleThingsClass
		{
			public string Role { get; set; }
			public List<ThingId> ThingIds { get; set; }
		}


	}
}
