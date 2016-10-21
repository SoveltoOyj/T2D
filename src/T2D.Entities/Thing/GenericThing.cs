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


		public Guid Creator_Fqdn { get; set; }
		public Guid Creator_US { get; set; }

		public Guid Archetype_Fqdn { get; set; }
		public Guid Archetype_US { get; set; }

		public Guid Parted_Fqdn { get; set; }
		public Guid Parted_US { get; set; }
	}
}
