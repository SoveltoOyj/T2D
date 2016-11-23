using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class ThingRoleDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ThingRole>();

			tbl.HasOne(e => e.Thing)
				.WithMany(t => t.ThingRoles)
				.HasForeignKey(e => e.ThingId)
				.OnDelete(DeleteBehavior.Cascade)
				;

			tbl.HasOne(e => e.Role)
				.WithMany()
				.HasForeignKey(e => e.RoleId)
				;

		}
	}
}
