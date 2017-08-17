using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ExtensionDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<Extension>();

			tbl
				.Property("US")
				.HasColumnType("nvarchar(512) COLLATE Finnish_Swedish_CS_AI")
//				.ForSqlServerHasColumnType("nvarchar(512) COLLATE Finnish_Swedish_CS_AI")
				;
		}

	}
}
