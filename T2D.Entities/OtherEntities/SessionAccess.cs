using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class SessionAccess : IEntity
	{
		public Guid Id { get; set; }

		public Guid SessionId { get; set; }

		public Guid ThingId { get; set; }
	
		public int RoleId { get; set; }

		#region Navigation Properties
		public Session Session { get; set; }
		public BaseThing Thing { get; set; }
		public Role Role { get; set; }

		#endregion
	}
}
