using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{
	public class GetRelationsRequest : BaseRequest
	{
	}
	public class GetRelationsResponse
	{
		public List<RelationsThings> RelationThings { get; set; }

		public class RelationsThings
		{
			public string Relation { get; set; }
			public List<IdTitle> Things { get; set; }

			public class IdTitle
			{
				public string ThingId { get; set; }
				public string Title{ get; set; }
			}
		}


	}
}
