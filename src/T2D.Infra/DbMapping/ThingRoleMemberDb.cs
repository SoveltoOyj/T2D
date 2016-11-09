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
				.HasForeignKey(e => e.ThingId)
				.OnDelete(DeleteBehavior.Cascade)
				;

			tbl.Property(e => e.ThingId)
					.IsRequired()
					;
		}
	}
}
