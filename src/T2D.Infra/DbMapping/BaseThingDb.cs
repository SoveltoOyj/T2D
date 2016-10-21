using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class BaseThingDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<BaseThing>();

			tbl.
				HasAlternateKey(e => new { e.Fqdn, e.US })
				.HasName("UI_Fqdn_UniqueString");

			modelBuilder.Entity<GenericThing>()
				.Property(e => e.Modified)
				.HasColumnType("DateTime")
				;
			modelBuilder.Entity<GenericThing>()
				.Property(e => e.Published)
				.HasColumnType("DateTime")
				;

		}
	}
}
