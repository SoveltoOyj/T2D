using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public static class SessionAccessDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<SessionAccess>();

			tbl.HasOne(e => e.Thing)
				.WithMany(t=>t.SessionAccesses)
				.HasForeignKey(e => e.ThingId)
				.OnDelete(DeleteBehavior.Restrict)
				;

			tbl.HasOne(e => e.Session)
				.WithMany(t => t.SessionAccesses)
				.HasForeignKey(e => e.SessionId)
				.OnDelete(DeleteBehavior.Restrict)
				;

		}
	}
}
