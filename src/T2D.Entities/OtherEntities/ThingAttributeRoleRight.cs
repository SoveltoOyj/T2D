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
    //viitaus siis sekä ThingAttribute- että ThingRole-luokkiin
    //pitääkö nämä luokat myös instantioida?
    public string ThingAttributeRoleId_CreatorUri { get; set; }
    public string ThingAttributeRoleId_UniqueString { get; set; }
   // public BaseThing Thing { get; set; }
    public long ThingAttribute_AttributeId { get; set; }
    // public Attribute Attribute { get; set; }
    public long ThingRole_RoleId { get; set; }
    public override string ToString()
    {
      return this.ToJson();
    }

  }
}
