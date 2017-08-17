using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class InventoryDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<Inventory>();

			tbl.
				HasAlternateKey(e => e.Fqdn)
				.HasName("UI_Fqdn")
				;

		}
	}
}
