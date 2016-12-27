﻿using System;
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