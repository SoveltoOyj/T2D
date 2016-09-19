using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingAttributeRoleRight:IEntity
    {
    public long Id { get; set; }
    public long ThingAttributeId { get; set; }
    public ThingAttribute ThingAttribute { get; set; }
    public long ThingRoleId { get; set; }
		public ThingRole ThingRole { get; set; }
		public RightEnum Rights { get; set; }
		public override string ToString()
    {
      return this.ToJson();
    }

  }
	[Flags]
	public enum RightEnum
	{
		Create = 1,
		Read = 2,
		Update = 4,
		Delete = 8,
	}
}
