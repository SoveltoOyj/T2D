using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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

		public DbSet<ThingRelation> ThingRelations { get; set; }

		public DbSet<Relation> Relations { get; set; }
		public DbSet<Role> Roles { get; set; }
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=T2D;Trusted_Connection=True;");
			optionsBuilder.UseSqlServer(@"Server=.;Database=T2D;Uid=t2d;pwd=Salainen;Trusted_Connection=False;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ThingBase>()
					.Property(p => p.Name)
					.HasMaxLength(50)
					;
			modelBuilder.Entity<ThingRelation>()
					.HasOne(tr => tr.Thing1)
					.WithMany(t => t.ThingRelations)
					.HasForeignKey(tr => tr.Thing1Id)
					.OnDelete(DeleteBehavior.Cascade)
					;

			modelBuilder.Entity<ThingRelation>()
				.HasOne(tr => tr.Relation)
				.WithMany()
				.HasForeignKey(tr=>tr.RelationId)
				;

			modelBuilder.Entity<ThingRelation>()
					.Property(tr => tr.Thing1Id)
					.IsRequired();
			modelBuilder.Entity<ThingRelation>()
					.Property(tr => tr.Thing2Id)
					.IsRequired();

		}


	}
}

