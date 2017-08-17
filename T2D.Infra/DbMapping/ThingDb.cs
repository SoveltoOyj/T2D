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
				.HasValue<IoThing>(value++)
				.HasValue<WalletThing>(value++)
				;

			
			modelBuilder.Entity<AuthenticationThing>().HasOne(e => e.PersonThing)
				.WithMany()
				.HasForeignKey(e => e.PersonThingId)
				.OnDelete(DeleteBehavior.Restrict)
				;

			modelBuilder.Entity<RegularThing>().HasOne(e => e.ArchetypeThing)
				.WithMany()
				.HasForeignKey(e => e.ArchetypeThingId)
				.OnDelete(DeleteBehavior.Restrict)
				;

			modelBuilder.Entity<RegularThing>().HasOne(e => e.LocationThing)
							.WithMany()
							.HasForeignKey(e => e.LocationThingId)
							.OnDelete(DeleteBehavior.Restrict)
							;

			modelBuilder.Entity<RegularThing>().HasOne(e => e.PreferredLocationThing)
							.WithMany()
							.HasForeignKey(e => e.PreferredLocationThingId)
							.OnDelete(DeleteBehavior.Restrict)
							;


			modelBuilder.Entity<GenericThing>().HasOne(e => e.PartedThing)
				.WithMany(e=>e.Parts)
				.HasForeignKey(e => e.PartedThingId)
				.OnDelete(DeleteBehavior.Restrict)
				;

			modelBuilder.Entity<BaseThing>()
				.Property("US")
				.HasColumnType("nvarchar(512) COLLATE Finnish_Swedish_CS_AI")
				//				.ForSqlServerHasColumnType("nvarchar(512) COLLATE Finnish_Swedish_CS_AI")
				;
		}

	}
}
