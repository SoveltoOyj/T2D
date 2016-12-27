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

		[Required]
		public Guid ThingId { get; set; }
		public BaseThing Thing { get; set; }

		[Required]
		public Guid ThingRoleId { get; set; }
		public ThingRole ThingRole { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
