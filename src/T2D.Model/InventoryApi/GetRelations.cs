using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{
	// request-luokan kantaluokka (demo git:n käytöstä, poista tämä turha kommentti)
	public abstract class GetAttributeRelationsBaseRequest
	{
		public string Session { get; set; }
		public string ThingId { get; set; }
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
			public List<ThingIdTitle> Things { get; set; }

			public class ThingIdTitle
			{
				public string ThingId { get; set; }
				public string Title{ get; set; }
			}
		}


	}
}
