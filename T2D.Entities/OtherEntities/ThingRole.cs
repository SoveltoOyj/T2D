using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Thing Roles.
	/// And also logging, will this Role access be logged
	/// </summary>
	public class ThingRole : IEntity
	{
		public Guid Id { get; set; }

		public Guid ThingId { get; set; }
		public int RoleId { get; set; }

		public bool Logging { get; set; }

		#region Navigation Properties
		public BaseThing Thing { get; set; }
		public Role Role { get; set; }

		#endregion
	}
}
