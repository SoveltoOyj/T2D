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
	public class Inventory : IEntity
	{
		/// <summary>
		/// ID 0001 is always this Inventory.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Globaly Unique fqdn for Inventory
		/// </summary>
		[StringLength(256), Required]
		public string Fqdn { get; set; }

		public string Title { get; set; }


		public Inventory()
		{
		}
	}
}
