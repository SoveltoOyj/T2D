using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ServiceDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<ServiceDefinition>();

			tbl
				.Property("Title")
				.HasColumnType("nvarchar(256) COLLATE Finnish_Swedish_CS_AI")
			//				.ForSqlServerHasColumnType("nvarchar(256) COLLATE Finnish_Swedish_CS_AI")
			;

		}

	}
}
