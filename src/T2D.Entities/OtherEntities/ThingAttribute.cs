using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
  public class ThingAttribute : IEntity
  {
    public long Id { get; set; }
    public string ThingId_CreatorUri { get; set; }
    public string ThingId_UniqueString { get; set; }
    public BaseThing Thing { get; set; }
    public long AttributeId { get; set; }
    public Attribute Attribute { get; set; }
    public bool Logging { get; set; }

    public override string ToString()
    {
      return this.ToJson();
    }


  }
}
