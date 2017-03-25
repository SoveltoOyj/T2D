using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public abstract class BaseThing:IThing
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Fqdn { get; set; }

		[StringLength(512), Required]
		public string US { get; set; }

		[StringLength(1024)]
		public string Title { get; set; }

		public List<ThingRelation> ThingRelations { get; set; }
		public List<ThingRoleMember> ThingRoleMembers { get; set; }
        public List<ThingRoleMember> MemeberThingRoleMembers { get; set; }
		public List<ThingRole> ThingRoles { get; set; }
		public List<ThingAttribute> ThingAttributes { get; set; }
		public List<Session> Sessions { get; set; }
        public List<SessionAccess> SessionAccesses { get; set; }
 
        public BaseThing()
        {
            ThingRelations = new List<ThingRelation>();
            MemeberThingRoleMembers = new List<ThingRoleMember>();
            ThingRoleMembers = new List<ThingRoleMember>();
            ThingRoles = new List<ThingRole>();
            ThingAttributes = new List<ThingAttribute>();
            Sessions = new List<Session>();
			SessionAccesses = new List<SessionAccess>();
        }

        public override string ToString()
        {
            return this.ToJson();
        }

    }
}
