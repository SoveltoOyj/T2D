using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class SessionAccess:IEntity
    {
    public Guid Id { get; set; }
		public Guid SessionId { get; set; }
		public Session Session { get; set; }

		public string Creator_ThingId { get; set; }
		public BaseThing Creator { get; set; }

		public int RoleId { get; set; }
		public Role Role { get; set; }
	}
}
