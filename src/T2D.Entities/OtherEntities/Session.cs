using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class Session : IEntity
	{
		public Guid Id { get; set; }
        [StringLength(512)]
		public string Token { get; set; }

		//public Guid InventoryId { get; set; }
		//public Inventory Inventory { get; set; }

		public Guid ExternalSessionId { get; set; }
		public Guid Remote_InventoryId { get; set; }

		public Guid EntryPoint_ThingId { get; set; }
		public BaseThing EntryPoint { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime? EndTime { get; set; }

		public List<SessionAccess> SessionAccesses { get; set; }
		public List<ThingAttributeRoleSessionAccess> ThingAttributeRoleSessionAccesses { get; set; }

		public Session()
		{
			SessionAccesses = new List<SessionAccess>();
			ThingAttributeRoleSessionAccesses = new List<ThingAttributeRoleSessionAccess>();
		}
	}
}
