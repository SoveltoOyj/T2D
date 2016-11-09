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

			tbl.HasOne(e => e.Thing1)
				.WithMany(t => t.ThingRelations)
				.HasForeignKey(e => e.Thing1_Id)
				.OnDelete(DeleteBehavior.Cascade)
				;

			tbl.HasOne(e => e.Relation)
				.WithMany()
				.HasForeignKey(e => e.RelationId)
				;


		}
	}
}
