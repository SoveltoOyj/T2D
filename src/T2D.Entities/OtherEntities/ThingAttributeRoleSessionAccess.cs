using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingAttributeRoleSessionAccess:IEntity
    {
    public Guid Id { get; set; }

		public Guid SessionId { get; set; }
        [StringLength(512)]
        public String SessionToken { get; set; }
        public Session Session { get; set; }

		public int ThingAttributeId { get; set; }
		public ThingAttribute ThingAttribute { get; set; }

		public int RoleId { get; set; }
		public Role Role{ get; set; }

		public DateTime Timestamp { get; set; }

		public RightEnum Rights { get; set; }
	}
}
