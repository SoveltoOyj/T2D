using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;

namespace T2D.Infra
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Create new T2D database (existing db will be deleted), press Y");
			var ki = Console.ReadKey();
			if (ki.Key.ToString().ToLower() != "y")
			{
				PrintData();
				return;
			}

			Console.WriteLine("\nCreating database ...");
			var dbc = new EfContext();
			//create database, add base data

			dbc.Database.EnsureDeleted();
			dbc.Database.EnsureCreated();

			Console.WriteLine("\nCreating data ...");
			dbc.Database.OpenConnection();
			try
			{
				//Enums
				AddEnumData(dbc, dbc.Relations, typeof(RelationEnum));
				AddEnumData(dbc, dbc.Roles, typeof(RoleEnum));

				//attributes
				AddAttributeData(dbc, typeof(AttributeEnum));

				string fqdn = "inv1.sovelto.fi";

				//Archetypethings
				dbc.ArchetypeThings.Add(new ArchetypeThing {Fqdn = fqdn, US = "ArcNb1", Title = "Archetype example", Modified = new DateTime(2016, 3, 23), Published = new DateTime(2016, 4, 13), Created = new DateTime(2014, 3, 23) });
				//AuthenticationThings
				var M100 = new AuthenticationThing { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0), Fqdn = fqdn, US = "M100", Title = "Matti, Facebook", };
				dbc.AuthenticationThings.Add(M100);
				dbc.SaveChanges();

				//things
				dbc.RegularThings.Add(new RegularThing
				{
					Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1),
					Fqdn = fqdn,
					US = "T1",
					Title = "MySuitcase",
					Created = new DateTime(2015, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 1,
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2016, 3, 23),
					Published = new DateTime(2016, 4, 13),
					Creator_Fqdn = M100.Fqdn,
					Creator_US=M100.US

				});
				dbc.SaveChanges();
				var T1 = dbc.Things.SingleOrDefault(t => t.Fqdn == fqdn && t.US == "T1");

				dbc.RegularThings.Add(new RegularThing
				{
					Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2),
					Fqdn = fqdn,
					US = "T2",
					Title = "A Container",
					Created = new DateTime(2015, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 2,
					Location_GPS = "123",
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2014, 3, 3),
					Published = new DateTime(2012, 4, 13)
				});
				dbc.SaveChanges();
				var T2 = dbc.Things.SingleOrDefault(t => t.Fqdn == fqdn && t.US == "T2");

				dbc.RegularThings.Add(new RegularThing
				{
					Fqdn = "inv1.sovelto.fi",
					US = "ThingNb3",
					Title = "A Thing",
					Created = new DateTime(2016, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 1,
					Location_GPS = "12443",
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2016, 3, 23),
					Published = new DateTime(2016, 4, 13),
					Parted_Fqdn= T2.Fqdn,
					Parted_US = T2.US
				});
				dbc.SaveChanges();

				//ThingRoleMember
				byte count = 1;
				// add omnipotent role to T0 for T0
				ThingRole tr = new ThingRole { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count), RoleId =(int)RoleEnum.Omnipotent, ThingId=M100.Id };
				dbc.ThingRoles.Add(tr);
				ThingRoleMember trm = new ThingRoleMember { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++), ThingId = M100.Id, ThingRoleId = tr.Id };
				dbc.ThingRoleMembers.Add(trm);

				//add owner role to T1 for T0
				tr = new ThingRole { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count), RoleId = (int)RoleEnum.Owner, ThingId = T1.Id };
				dbc.ThingRoles.Add(tr);
				trm = new ThingRoleMember { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++), ThingId = M100.Id, ThingRoleId = tr.Id };
				dbc.ThingRoleMembers.Add(trm);

				//add Belongings role to T2 for T1
				tr = new ThingRole { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count), RoleId = (int)RoleEnum.Belongings, ThingId = T2.Id };
				dbc.ThingRoles.Add(tr);
				trm = new ThingRoleMember { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++), ThingId = T1.Id, ThingRoleId = tr.Id };
				dbc.ThingRoleMembers.Add(trm);

				dbc.SaveChanges();


				//ThingRelation
				count = 1;
				dbc.ThingRelations.Add(new ThingRelation
				{
					Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++),
					Thing1_Id = M100.Id,
					Thing2_Fqdn = T1.Fqdn,
					Thing2_US = T1.US,
					RelationId = (int)RelationEnum.Belongings
				});
				dbc.ThingRelations.Add(new ThingRelation
				{
					Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++),
					Thing1_Id = M100.Id,
					Thing2_Fqdn = T1.Fqdn,
					Thing2_US = T1.US,
					RelationId = (int)RelationEnum.RoleIn
				});
				dbc.ThingRelations.Add(new ThingRelation
				{
					Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count++),
					Thing1_Id = T1.Id,
					Thing2_Fqdn = T2.Fqdn,
					Thing2_US = T2.US,
					RelationId = (int)RelationEnum.ContainedBy
				});


				dbc.SaveChanges();
				count = 1;
				// test session data
				// add session 00000001 for T0, no sessionaccess yet
				dbc.Sessions.Add(new Session { Id = new Guid(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, count), StartTime = DateTime.UtcNow, EntryPoint_ThingId = M100.Id });
				dbc.SaveChanges();

			}

			finally
			{
				dbc.Database.CloseConnection();
			}


			Console.WriteLine("\nDone, Press enter to print some of the data.");
			Console.ReadLine();
			PrintData();
		}


		private static void PrintData()
		{
			using (var dbc = new EfContext())
			{
				Console.WriteLine("ArchetypeThings, version 1");
				foreach (var item in dbc.ArchetypeThings)
				{
					Console.WriteLine($"  {item.US}");
				}

				Console.WriteLine("\nArchetypeThings, version 2");
				foreach (var item in dbc.Things.OfType<ArchetypeThing>())
				{
					Console.WriteLine($"  {item.US}");
				}

				Console.WriteLine("\nEager Loading");
				foreach (var item in dbc.Things.Include(e => e.ThingRelations).ThenInclude(e => e.Relation))
				{
					Console.WriteLine($"  {item.US}");
					foreach (var tr in item.ThingRelations)
					{
						Console.WriteLine($"      Relation to: {tr.Thing2_Fqdn}/{tr.Thing2_US} Relation:{tr.Relation}");
					}
					Console.WriteLine();
				}
			}
		}

		private static void AddEnumData<TEntity>(EfContext dbc, DbSet<TEntity> dbSet, Type enumType)
			where TEntity : class, IEnumEntity, new()
		{
			//			T2D.Infra.EfContext dbc = ((IInfrastructure<IServiceProvider>)dbSet).GetService<DbContext>() as T2D.Infra.EfContext;

			var entityType = dbc.Model.FindEntityType(typeof(TEntity));
			var table = entityType.SqlServer();
			var tableName = table.Schema + "." + table.TableName;

			dbc.Database.ExecuteSqlCommand($"Set identity_insert {tableName} on;");
			foreach (var item in Enum.GetNames(enumType))
			{
				dbSet.Add(new TEntity { Id = (int)Enum.Parse(enumType, item, false), Name = item });
			}
			dbc.SaveChanges();
			dbc.Database.ExecuteSqlCommand($"Set identity_insert {tableName} off;");
		}

		private static void AddAttributeData(EfContext dbc, Type enumType)
		{
			var entityType = dbc.Model.FindEntityType(typeof(Entities.Attribute));
			var table = entityType.SqlServer();
			var tableName = table.Schema + "." + table.TableName;

			dbc.Database.ExecuteSqlCommand($"Set identity_insert {tableName} on;");
			foreach (var item in Enum.GetNames(enumType))
			{
				//ToDo: Add min/max/Pattern and so on...
				dbc.Attributes.Add(new Entities.Attribute { Id = (int)Enum.Parse(enumType, item, false), Name = item });
			}
			dbc.SaveChanges();
			dbc.Database.ExecuteSqlCommand($"Set identity_insert {tableName} off;");
		}
	}
}
