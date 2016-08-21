using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;

namespace T2D.Infra
{
	public class EfContext : DbContext
	{
		public DbSet<ThingBase> Things { get; set; }
		public DbSet<AliasThing> AliasThings { get; set; }
		public DbSet<GenericThing> GenericThings { get; set; }
		public DbSet<AuthenticationThing> AuthenticationThings { get; set; }
		public DbSet<RegularThing> RegularThings { get; set; }
		public DbSet<ArchetypeThing> ArchetypeThings { get; set; }
		public DbSet<ArchivedThing> ArchivedThings { get; set; }


		public DbSet<Relation> Relations { get; set; }
		public DbSet<Role> Roles { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=T2D;Trusted_Connection=True;");

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ThingBase>()
					.Property(p => p.Name)
					.HasMaxLength(50)
					;

		}


	}
}

