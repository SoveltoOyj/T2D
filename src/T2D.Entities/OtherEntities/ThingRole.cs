using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingRole:IEntity
    {
    public string Id { get; set; }

		public string Creator_ThingId { get; set; }
		//public string ThingId_CreatorUri { get; set; }
		//public string ThingId_UniqueString { get; set; }
    public BaseThing Thing { get; set; }
    public bool Logging { get; set; }

    public string RoleId { get; set; }
    public Role Role { get; set; }

  }
}
