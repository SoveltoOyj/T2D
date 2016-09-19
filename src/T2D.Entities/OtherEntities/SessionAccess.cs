using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class SessionAccess:IEntity
    {
    public long Id { get; set; }
		public long SessionId { get; set; }
		public Session Session { get; set; }
		public string ThingId_CreatorUri { get; set; }
    public string ThingId_UniqueString { get; set; }
		public long RoleId { get; set; }
		public Role Role { get; set; }
	}
}
