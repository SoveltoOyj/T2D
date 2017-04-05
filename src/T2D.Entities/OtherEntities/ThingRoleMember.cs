using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingRoleMember:IEntity
    {
		public Guid Id { get; set; }

		public Guid ThingId { get; set; }

		public Guid ThingRoleId { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}

		#region Navigation Properties
		public BaseThing Thing { get; set; }
		public ThingRole ThingRole { get; set; }
		#endregion
	}
}
