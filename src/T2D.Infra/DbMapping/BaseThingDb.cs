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

			tbl.HasKey(t => new { t.Id_CreatorUri, t.Id_UniqueString })
					;

			modelBuilder.Entity<GenericThing>()
				.Property(e => e.Modified)
				.ForSqlServerHasColumnType("DateTime")
				;
			modelBuilder.Entity<GenericThing>()
				.Property(e => e.Published)
				.ForSqlServerHasColumnType("DateTime")
				;


		}
	}
}
