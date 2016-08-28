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
		public string CreatorThingId_CreatorUri { get; set; }
		public string CreatorThingId_UniqueString { get; set; }
		public string ArchetypeThingId_CreatorUri { get; set; }
		public string ArchetypeThingId_UniqueString { get; set; }
		public string PartedThingId_CreatorUri { get; set; }
		public string PartedThingId_UniqueString { get; set; }
		

	}
}
