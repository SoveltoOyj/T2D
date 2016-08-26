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
				.HasForeignKey(e => new
				{
					e.Thing1_Id_CreatorUri,
					e.Thing1_Id_UniqueString
				})
				.OnDelete(DeleteBehavior.Cascade)
				;

			tbl.HasOne(e => e.Relation)
				.WithMany()
				.HasForeignKey(e => e.RelationId)
				;

			tbl.Property(e => e.Thing1_Id_CreatorUri)
					.IsRequired()
					;

			tbl.Property(e => e.Thing1_Id_UniqueString)
					.IsRequired()
					;

			tbl.Property(e => e.Thing2_Id_CreatorUri)
					.IsRequired()
					;

			tbl.Property(e => e.Thing2_Id_UniqueString)
				.IsRequired()
				;

		}
	}
}
