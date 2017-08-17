using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class SessionDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<Session>();

			tbl.HasOne(e => e.EntryPoint)
					.WithMany(t => t.Sessions)
					.HasForeignKey(e => e.EntryPoint_ThingId)
					.OnDelete(DeleteBehavior.Restrict)
					;

		}

	}
}
