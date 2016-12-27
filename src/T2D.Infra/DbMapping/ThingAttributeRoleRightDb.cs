using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ThingAttributeRoleRightDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ThingAttributeRoleRight>();

			tbl.HasOne(e => e.ThingAttribute)
					.WithMany(t => t.ThingAttributeRoleRights)
					.HasForeignKey(e => e.ThingAttributeId)
					.OnDelete(DeleteBehavior.Restrict)
					;

			tbl.HasOne(e => e.ThingRole)
					 .WithMany()
					 .HasForeignKey(e => e.ThingAttributeId)
					 .OnDelete(DeleteBehavior.Restrict)
					 ;

		}

	}
}
