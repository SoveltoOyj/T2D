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

			tbl.Property(e => e.CreatorFQDN)
				.HasMaxLength(256)
				.IsRequired(true)
				;
			tbl.Property(e => e.UniqueString)
				.HasMaxLength(1024)
				.IsRequired(true)
				;

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
