using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Log of thing accesses. Which Attribute by which role and AccessType in which Session
	/// </summary>
	public class ThingAttributeRoleSessionAccess : IEntity
	{
		public Guid Id { get; set; }

		public Guid SessionId { get; set; }

		public int ThingAttributeId { get; set; }

		public int RoleId { get; set; }

		public DateTime Timestamp { get; set; }

		//ToDo: is this name OK?
		public RightFlag AccessType { get; set; }

		#region Navigation Properties
		public Session Session { get; set; }
		public ThingAttribute ThingAttribute { get; set; }
		public Role Role { get; set; }
		#endregion
	}
}
