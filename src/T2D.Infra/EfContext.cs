using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
		#region Things
		public DbSet<BaseThing> Things { get; set; }
		public DbSet<AliasThing> AliasThings { get; set; }
		public DbSet<GenericThing> GenericThings { get; set; }
		public DbSet<AuthenticationThing> AuthenticationThings { get; set; }
		public DbSet<RegularThing> RegularThings { get; set; }
		public DbSet<ArchetypeThing> ArchetypeThings { get; set; }
		public DbSet<IoThing>IoTThings { get; set; }
		public DbSet<WalletThing> WalletThings { get; set; }
		#endregion

		public DbSet<ThingRelation> ThingRelations { get; set; }
		public DbSet<ThingRoleMember> ThingRoleMembers { get; set; }
		public DbSet<ThingRole> ThingRoles { get; set; }
		public DbSet<Relation> Relations { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<Entities.Attribute> Attributes { get; set; }
		public DbSet<ThingAttribute> ThingAttributes { get; set; }
		public DbSet<ThingAttributeRoleRight> ThingAttributeRoleRights { get; set; }
		public DbSet<LocationType> LocationTypes { get; set; }
		public DbSet<FunctionalStatus> Status { get; set; }

		public DbSet<Entities.Inventory> Inventories { get; set; }

		public DbSet<Entities.Session> Sessions { get; set; }
		public DbSet<Entities.SessionAccess> SessionAccesses { get; set; }

		public DbSet<Extension> Extensions{ get; set; }

		#region ServiceRequest
		public DbSet<ServiceDefinition> ServiceDefinitions { get; set; }

		public DbSet<ActionDefinition> ActionDefinitions { get; set; }
		public DbSet<GenericAction> GenericActions { get; set; }
		public DbSet<PaymentRequestAction> PaymentRequestActions { get; set; }
		public DbSet<ReceiptRequestAction> ReceiptRequestActions { get; set; }
		public DbSet<IoTBotRequestAction> IoTBotRequestActions { get; set; }
		public DbSet<ActionDefinition> ServiceRequestActions { get; set; }


		public DbSet<ServiceStatus> ServiceStatuses { get; set; }

		public DbSet<ActionStatus> ActionStatuses { get; set; }
		#endregion


		public override int SaveChanges()
		{
			var modifiedEntries = ChangeTracker.Entries<IAuditableEntity>()
					 .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

			foreach (EntityEntry<IAuditableEntity> entry in modifiedEntries)
			{
				entry.Entity.Modified = DateTime.UtcNow;

				if (entry.State == EntityState.Added)
				{
					entry.Entity.Created = DateTime.Now;
				}
			}
			return base.SaveChanges();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(local);Database=T2D;Trusted_Connection=True;");
			//optionsBuilder.UseSqlServer(@"Server=.;Database=T2D;Uid=t2d;pwd=Salainen;Trusted_Connection=False;");
			//optionsBuilder.UseSqlServer(@"Server=tcp:t2dahti.database.windows.net,1433;Database=t2d;Uid=ahti;pwd=Salainen1!;Persist Security Info = False;; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			ThingDb.SetDbMapping(modelBuilder);

			InventoryDb.SetDbMapping(modelBuilder);
			BaseThingDb.SetDbMapping(modelBuilder);
			AttributeDb.SetDbMapping(modelBuilder);
			RoleDb.SetDbMapping(modelBuilder);
			ThingRelationDb.SetDbMapping(modelBuilder);
			ThingRoleMemberDb.SetDbMapping(modelBuilder);
			SessionDb.SetDbMapping(modelBuilder);
			SessionAccessDb.SetDbMapping(modelBuilder);
			ThingRoleDb.SetDbMapping(modelBuilder);
			ThingAttributeDb.SetDbMapping(modelBuilder);
			ThingAttributeRoleRightDb.SetDbMapping(modelBuilder);

			ActionDefinitionDb.SetDbMapping(modelBuilder);
			ServiceStatusDb.SetDbMapping(modelBuilder);
			ActionStatusDb.SetDbMapping(modelBuilder);


		}


	}
}

