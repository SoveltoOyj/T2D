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
				dbc.ArchetypeThings.Add(new ArchetypeThing {CreatorFQDN = fqdn, UniqueString = "ArcNb1", Title = "Archetype example", Modified = new DateTime(2016, 3, 23), Published = new DateTime(2016, 4, 13), Created = new DateTime(2014, 3, 23) });
				//AuthenticationThings
				dbc.AuthenticationThings.Add(new AuthenticationThing {CreatorFQDN = fqdn, UniqueString = "T0", Title = "Matti, Facebook", });
				dbc.SaveChanges();
				var T0 = dbc.AuthenticationThings.SingleOrDefault(t => t.CreatorFQDN == fqdn && t.UniqueString == "T0");

				//things
				dbc.RegularThings.Add(new RegularThing
				{
					CreatorFQDN = fqdn,
					UniqueString = "T1",
					Title = "MySuitcase",
					Created = new DateTime(2015, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 1,
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2016, 3, 23),
					Published = new DateTime(2016, 4, 13),
					Creator_ThingId = T0.Id,
				});
				dbc.SaveChanges();
				var T1 = dbc.Things.SingleOrDefault(t => t.CreatorFQDN == fqdn && t.UniqueString == "T1");

				dbc.RegularThings.Add(new RegularThing
				{
					CreatorFQDN = fqdn,
					UniqueString = "T2",
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
				var T2 = dbc.Things.SingleOrDefault(t => t.CreatorFQDN == fqdn && t.UniqueString == "T2");

				dbc.RegularThings.Add(new RegularThing
				{
					CreatorFQDN = "inv1.sovelto.fi",
					UniqueString = "ThingNb3",
					Title = "A Thing",
					Created = new DateTime(2016, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 1,
					Location_GPS = "12443",
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2016, 3, 23),
					Published = new DateTime(2016, 4, 13),
					Parted_ThingId= T2.Id,
				});
				dbc.SaveChanges();

				//ThingRoleMember

				dbc.SaveChanges();

				//ThingRelation
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id = T1.Id,
					Thing2_Id = T2.Id,
					RelationId = (int)RelationEnum.Belongings
				});
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id = T1.Id,
					Thing2_Id = T2.Id,
					RelationId = (int)RelationEnum.RoleIn
				});
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id = T1.Id,
					Thing2_Id = T2.Id,
					RelationId = (int)RelationEnum.ContainedBy
				});


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
					Console.WriteLine($"  {item.UniqueString}");
				}

				Console.WriteLine("\nArchetypeThings, version 2");
				foreach (var item in dbc.Things.OfType<ArchetypeThing>())
				{
					Console.WriteLine($"  {item.UniqueString}");
				}

				Console.WriteLine("\nEager Loading");
				foreach (var item in dbc.Things.Include(e => e.ThingRelations).ThenInclude(e => e.Relation))
				{
					Console.WriteLine($"  {item.UniqueString}");
					foreach (var tr in item.ThingRelations)
					{
						Console.WriteLine($"      Relation to: {tr.Thing2_Id} Relation:{tr.Relation}");
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
