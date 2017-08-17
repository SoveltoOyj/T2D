using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Rights per Thing Attribute Role.
	/// </summary>
	public class ThingAttributeRoleRight : IEntity
	{
		public Guid Id { get; set; }

		public Guid ThingAttributeId { get; set; }

		public Guid ThingRoleId { get; set; }

		public RightFlag Rights { get; set; }
		public override string ToString()
		{
			return this.ToJson();
		}

		#region Navigation Properties
		public ThingAttribute ThingAttribute { get; set; }
		public ThingRole ThingRole { get; set; }
		#endregion
	}

}
