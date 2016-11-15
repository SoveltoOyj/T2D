using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
  public class ThingAttribute : IEntity
  {
    public Guid Id { get; set; }

	public Guid ThingId { get; set; }
    public BaseThing Thing { get; set; }

	public int AttributeId { get; set; }
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
