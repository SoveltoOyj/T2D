﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
    public class GenericThing:BaseThing
    {
		public DateTime Published { get; set; }
		public DateTime Modified { get; set; }
		public string ArchetypeThingId_CreatorUri { get; set; }
		public string ArchetypeThingId_UniqueString { get; set; }
		public string PartedThingId_CreatorUri { get; set; }
		public string PartedThingId_UniqueString { get; set; }
		public List<ThingRoleMemeber> ThingRoleMembers { get; set; }
		public List<ThingRoleMemeber> MemeberThingRoleMembers { get; set; }
		public GenericThing()
		{
			ThingRoleMembers = new List<ThingRoleMemeber>();
			MemeberThingRoleMembers = new List<ThingRoleMemeber>();
		}

	}
}
