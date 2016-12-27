using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	public class Inventory : IEntity
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Fqdn { get; set; }

		public string Name { get; set; }

		public List<Session> Sessions { get; set; }

		public Inventory()
		{
			Sessions = new List<Session>();
		}
	}
}
