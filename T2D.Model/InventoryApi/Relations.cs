using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{

	/// <summary>
	/// These relations will override all existing relations.
	/// </summary>
	public class SetRelationsRequest : BaseRequest
	{
		public List<RelationsThingIds> RelationThings { get; set; }
	}

	/// <summary>
	/// Relations and ThingIds in this relation.
	/// </summary>
	public class RelationsThingIds
	{
		public string Relation { get; set; }
		public List<string> Things { get; set; }

	}




	public class GetRelationsRequest : BaseRequest
	{
	}
	public class GetRelationsResponse
	{
		public List<RelationsThings> RelationThings { get; set; }
	}
	public class RelationsThings
	{
		public string Relation { get; set; }
		public List<ThingIdTitle> Things { get; set; }

		public class ThingIdTitle
		{
			public string ThingId { get; set; }
			public string Title { get; set; }
		}
	}


}
