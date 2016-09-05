using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class ThingRole:IEntity
    {
    public long Id { get; set; }
    
    public string ThingId_CreatorUri { get; set; }
    public string ThingId_UniqueString { get; set; }
    public BaseThing Thing { get; set; }
    public bool Logging { get; set; }

    public long RoleId { get; set; }
    public Role Role { get; set; }

  }
}
