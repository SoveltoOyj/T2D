using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingRole:IEntity
    {
    public Guid Id { get; set; }

	public Guid ThingId { get; set; }
    public BaseThing Thing { get; set; }

	public bool Logging { get; set; }

    public int RoleId { get; set; }
    public Role Role { get; set; }
  }
}
