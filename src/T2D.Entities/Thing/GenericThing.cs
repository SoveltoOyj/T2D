using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
    public class GenericThing:BaseThing
    {
		public DateTime? Created { get; set; }
		public DateTime? Published { get; set; }
		public DateTime? Modified { get; set; }
		public Guid Creator_ThingId { get; set; }
		public Guid Archetype_ThingId { get; set; }
		public Guid Parted_ThingId { get; set; }
	}
}
