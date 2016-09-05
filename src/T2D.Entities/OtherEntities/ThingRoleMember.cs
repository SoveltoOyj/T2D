using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingRoleMember:IEntity
    {
		public long Id { get; set; }

		public string ThingId_CreatorUri { get; set; }
		public string ThingId_UniqueString { get; set; }
		public BaseThing Thing { get; set; }

		public long ThingRoleId { get; set; }
		public ThingRole ThingRole { get; set; }

		public string Member_ThingId_CreatorUri { get; set; }
		public string Member_ThingId_UniqueString { get; set; }
		public BaseThing Member { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}


	}
}
