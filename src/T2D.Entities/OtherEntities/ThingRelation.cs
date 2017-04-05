using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class ThingRelation : IEntity
	{
		public Guid Id { get; set; }

		public Guid FromThingId { get; set; }

		public int RelationId { get; set; }

		public Guid ToThingId { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}

		#region Navigation Properties
		public BaseThing FromThing { get; set; }
		public BaseThing ToThing { get; set; }
		public Relation Relation { get; set; }
		#endregion
	}
}
