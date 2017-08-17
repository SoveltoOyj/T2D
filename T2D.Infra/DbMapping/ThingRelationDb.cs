using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class ThingRelationDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ThingRelation>();

			tbl.
				HasOne(e => e.FromThing)
						.WithMany(e => e.FromThingRelations)
						.HasForeignKey(e => e.FromThingId)
						.OnDelete(DeleteBehavior.Restrict)
						;

			tbl.
				HasOne(e => e.ToThing)
						.WithMany(e => e.ToThingRelations)
						.HasForeignKey(e => e.ToThingId)
						.OnDelete(DeleteBehavior.Restrict)
						;


		}
	}
}
