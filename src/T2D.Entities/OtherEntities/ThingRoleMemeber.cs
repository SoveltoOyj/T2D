using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
    public class ThingRoleMemeber
    {
		public long Id { get; set; }

		public string ThingId_CreatorUri { get; set; }
		public string ThingId_UniqueString { get; set; }
		public GenericThing Thing { get; set; }

		public long ThingRoleId { get; set; }
		//public ThingRole ThingRole { get; set; }

		public string Member_ThingId_CreatorUri { get; set; }
		public string Member_ThingId_UniqueString { get; set; }
		public GenericThing Member { get; set; }


	}
}
