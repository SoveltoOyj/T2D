using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ServiceRequestActionDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ServiceRequestAction>();

			tbl.HasOne(e => e.Thing)
				.WithMany()
				.OnDelete(DeleteBehavior.Restrict)
				;
			tbl.HasOne(e => e.ServiceRequest)
							.WithMany()
							.OnDelete(DeleteBehavior.Restrict)
							;

		}

	}
}
