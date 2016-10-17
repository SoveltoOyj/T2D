using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class ThingRelation : IEntity
	{
		public string Id { get; set; }

		public string Thing1_Id { get; set; }
		//public string Thing1_Id_CreatorUri { get; set; }
		//public string Thing1_Id_UniqueString { get; set; }
		public BaseThing Thing1 { get; set; }

		public int RelationId { get; set; }
		public Relation Relation { get; set; }

		public string Thing2_Id { get; set; }
		//public string Thing2_Id_CreatorUri { get; set; }
		//public string Thing2_Id_UniqueString { get; set; }

		public bool Thing2IsLocal { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}


	}
}
