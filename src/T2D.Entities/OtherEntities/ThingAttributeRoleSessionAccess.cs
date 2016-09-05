using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingAttributeRoleSessionAccess:IEntity
    {
    public long Id { get; set; }
		public long SessionId { get; set; }
		public Session Session { get; set; }
		public long ThingAttributeId { get; set; }
		public ThingAttribute ThingAttribute { get; set; }
		public DateTime Timestamp { get; set; }

		public RightEnum AccessType { get; set; }
	}
}
