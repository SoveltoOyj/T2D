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
		public DbSet<BaseThing> Things { get; set; }
		public DbSet<AliasThing> AliasThings { get; set; }
		public DbSet<GenericThing> GenericThings { get; set; }
		public DbSet<AuthenticationThing> AuthenticationThings { get; set; }
		public DbSet<RegularThing> RegularThings { get; set; }
		public DbSet<ArchetypeThing> ArchetypeThings { get; set; }
		public DbSet<ArchivedThing> ArchivedThings { get; set; }

		public DbSet<ThingRelation> ThingRelations { get; set; }
		public DbSet<ThingRoleMember> ThingRoleMembers { get; set; }

		public DbSet<Relation> Relations { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Entities.Attribute> Attributes { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			//			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=T2D;Trusted_Connection=True;");
			optionsBuilder.UseSqlServer(@"Server=.;Database=T2D;Uid=t2d;pwd=Salainen;Trusted_Connection=False;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			BaseThingDb.SetDbMapping(modelBuilder);
			ThingRelationDb.SetDbMapping(modelBuilder);
			ThingRoleMemberDb.SetDbMapping(modelBuilder);

		}


	}
}

