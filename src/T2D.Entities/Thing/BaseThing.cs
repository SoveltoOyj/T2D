using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// All things, things in this Inventory as well as things that live in other Inventories and this
	/// Inventory Things have relations to those.
	/// </summary>
	public abstract class BaseThing : IThing
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Fqdn { get; set; }

		[StringLength(512), Required]
		public string US { get; set; }


		#region Navigation Properties
		public List<ThingRelation> ToThingRelations { get; set; }
		public List<ThingRelation> FromThingRelations { get; set; }
		public List<ThingRoleMember> ThingRoleMembers { get; set; }
		public List<ThingRoleMember> MemeberThingRoleMembers { get; set; }
		public List<ThingRole> ThingRoles { get; set; }
		public List<ThingAttribute> ThingAttributes { get; set; }
		public List<SessionAccess> SessionAccesses { get; set; }
		#endregion


		public BaseThing()
		{
			ToThingRelations = new List<ThingRelation>();
			FromThingRelations = new List<ThingRelation>();
			MemeberThingRoleMembers = new List<ThingRoleMember>();
			ThingRoleMembers = new List<ThingRoleMember>();
			ThingRoles = new List<ThingRole>();
			ThingAttributes = new List<ThingAttribute>();
			SessionAccesses = new List<SessionAccess>();
		}

		public override string ToString()
		{
			return this.ToJson();
		}

	}
	
}
