using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class ThingRelation : IEntity
	{
		public long Id { get; set; }

		public string Name { get; set; }

		public long Thing1Id { get; set; }
		public ThingBase Thing1 { get; set; }

		public long Thing2Id { get; set; }
		public bool Thing2IsLocal { get; set; }

		public long RelationId { get; set; }
		public Relation Relation { get; set; }

	}
}
