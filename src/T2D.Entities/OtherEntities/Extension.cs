using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// This Inventory metadata and all trusted Inventories
	/// Inventory Metadata
	/// </summary>
	public class Extension : IEntity
	{
		public Guid Id { get; set; }

		public Guid ThingId { get; set; }

		public string Data { get; set; }

		#region Navigation Properties
		public BaseThing Thing { get; set; }
		#endregion
		public Extension()
		{
		}
	}
}
