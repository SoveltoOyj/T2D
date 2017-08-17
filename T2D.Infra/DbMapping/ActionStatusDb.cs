using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ActionStatusDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ActionStatus>();

			tbl.HasOne(e => e.ServiceStatus)
				.WithMany(t => t.ActionStatuses)
				.HasForeignKey(e => e.ServiceStatusId)
				.OnDelete(DeleteBehavior.Restrict)
				;
		}

	}
}
