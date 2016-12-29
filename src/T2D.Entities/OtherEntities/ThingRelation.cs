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

		public Guid Thing1_Id { get; set; }
		public BaseThing Thing1 { get; set; }

		public int RelationId { get; set; }
		public Relation Relation { get; set; }

		[StringLength(256), Required]
		public string Thing2_Fqdn { get; set; }
		[StringLength(512), Required]
		public string Thing2_US { get; set; }

		public bool Thing2IsLocal { get; set; }
// use index-search instead?

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
