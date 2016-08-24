using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class ThingRelation : IEntity
	{
		public long Id { get; set; }

		public string Thing1_Id_CreatorUri { get; set; }
		public string Thing1_Id_UniqueString { get; set; }
		public BaseThing Thing1 { get; set; }

		public long RelationId { get; set; }
		public Relation Relation { get; set; }

		public string Thing2_Id_CreatorUri { get; set; }
		public string Thing2_Id_UniqueString { get; set; }

		public bool Thing2IsLocal { get; set; }


	}
}
