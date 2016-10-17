using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class ThingRoleMemberDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ThingRoleMember>();

			tbl.HasOne(e => e.Thing)
				.WithMany(t => t.ThingRoleMembers)
				.HasForeignKey(e => new
				{
					e.Creator_ThingId,
					//e.ThingId_CreatorUri,
					//e.ThingId_UniqueString
				})
				.OnDelete(DeleteBehavior.Cascade)
				;


			tbl.Property(e => e.Creator_ThingId)
					.IsRequired()
					;
			//tbl.Property(e => e.ThingId_CreatorUri)
			//		.IsRequired()
			//		;

			//tbl.Property(e => e.ThingId_UniqueString)
			//		.IsRequired()
			//		;

			tbl.HasOne(e => e.Member)
				.WithMany(t => t.MemeberThingRoleMembers)
				.HasForeignKey(e => new
				{
					e.Member_Creator_ThingId,
					//e.Member_ThingId_CreatorUri,
					//e.Member_ThingId_UniqueString
				})
				.OnDelete(DeleteBehavior.Restrict)
				;

			tbl.Property(e => e.Member_Creator_ThingId)
					.IsRequired()
					;

			//tbl.Property(e => e.Member_ThingId_CreatorUri)
			//		.IsRequired()
			//		;

			//tbl.Property(e => e.Member_ThingId_UniqueString)
			//		.IsRequired()
			//		;

		}
	}
}
