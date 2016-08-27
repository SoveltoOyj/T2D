﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	public class BaseThing:IThingEntity
	{
		public string Id_CreatorUri { get; set; }
		public string Id_UniqueString { get; set; }
		public string Title { get; set; }
		public List<ThingRelation> ThingRelations { get; set; }
		public List<ThingRoleMember> ThingRoleMembers { get; set; }
		public List<ThingRoleMember> MemeberThingRoleMembers { get; set; }
		public BaseThing()
		{
			ThingRoleMembers = new List<ThingRoleMember>();
			MemeberThingRoleMembers = new List<ThingRoleMember>();
			ThingRelations = new List<ThingRelation>();
		}


	}
}
