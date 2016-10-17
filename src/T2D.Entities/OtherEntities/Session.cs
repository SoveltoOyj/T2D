using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
    public class Session:IEntity
    {
    public string Id { get; set; }
		public string InventoryId { get; set; }
		//public Inventory Inventory { get; set; }
		public string ExternalSessionId { get; set; }
		public string Remote_InventoryId { get; set; }
		public string EntryPoint_ThingId { get; set; }
		//public string EntryPointThingId_CreatorUri { get; set; }
		//public string EntryPointThingId_UniqueString { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public List<SessionAccess> SessionAccesses { get; set; }
		public List<ThingAttributeRoleSessionAccess> ThingAttributeRoleSessionAccesses { get; set; }
		public Session()
		{
			SessionAccesses = new List<SessionAccess>();
			ThingAttributeRoleSessionAccesses = new List<ThingAttributeRoleSessionAccess>();
		}

	}
}
