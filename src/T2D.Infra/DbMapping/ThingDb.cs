using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using T2D.Entities;

namespace T2D.Infra
{
	public class ThingDb
	{
		public static void SetDbMapping(ModelBuilder modelBuilder)
		{
			var tbl = modelBuilder.Entity<BaseThing>();

			int value = 1;
			tbl
				.HasDiscriminator<int>("Discriminator")
				.HasValue<AliasThing>(value++)
				.HasValue<GenericThing>(value++)
				.HasValue<AuthenticationThing>(value++)
				.HasValue<RegularThing>(value++)
				.HasValue<ArchetypeThing>(value++)
				.HasValue<ArchivedThing>(value++)
				.HasValue<IoThing>(value++)
				.HasValue<WalletThing>(value++)
				;
		}

	}
}
