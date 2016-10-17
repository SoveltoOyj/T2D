using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
  public class ThingAttribute : IEntity
  {
    public string Id { get; set; }
		public string Creator_ThingId { get; set; }
		//public string ThingId_CreatorUri { get; set; }
		//public string ThingId_UniqueString { get; set; }
    public BaseThing Thing { get; set; }
    public string AttributeId { get; set; }
    public Attribute Attribute { get; set; }
    public bool Logging { get; set; }
		public List<ThingAttributeRoleRight> ThingAttributeRoleRights { get; set; }
		public List<ThingAttributeRoleSessionAccess> ThingAttributeRoleSessionAccesses { get; set; }
		public override string ToString()
    {
      return this.ToJson();
    }
		public ThingAttribute()
		{
			ThingAttributeRoleRights = new List<ThingAttributeRoleRight>();
			ThingAttributeRoleSessionAccesses = new List<ThingAttributeRoleSessionAccess>();
		}

  }
}
