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

				//Archetypethings
				dbc.ArchetypeThings.Add(new ArchetypeThing { Id_CreatorUri = "sovelto.fi/inventory", Id_UniqueString = "ArcNb1", Title = "Archetype example", Modified = new DateTime(2016, 3, 23), Published = new DateTime(2016, 4, 13), Created = new DateTime(2014, 3, 23) });
				//AuthenticationThings
				dbc.AuthenticationThings.Add(new AuthenticationThing { Id_CreatorUri = "sovelto.fi/inventory", Id_UniqueString = "T0", Title = "Matti, Facebook", });
				//things
				dbc.RegularThings.Add(new RegularThing
				{
					Id_CreatorUri = "sovelto.fi/inventory",
					Id_UniqueString = "T1",
					Title = "MySuitcase",
					Created = new DateTime(2015, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 1,
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2016, 3, 23),
					Published = new DateTime(2016, 4, 13),
					CreatorThingId_CreatorUri = "sovelto.fi/inventory",
					CreatorThingId_UniqueString = "T0",
				});
				dbc.RegularThings.Add(new RegularThing
				{
					Id_CreatorUri = "sovelto.fi/inventory",
					Id_UniqueString = "T2",
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

				dbc.RegularThings.Add(new RegularThing
				{
					Id_CreatorUri = "sovelto.fi/inventory",
					Id_UniqueString = "ThingNb3",
					Title = "A Thing",
					Created = new DateTime(2016, 3, 1),
					IsLocalOnly = true,
					LocationTypeId = 1,
					Location_GPS = "12443",
					Logging = true,
					PreferredLocation_Id = 1,
					Modified = new DateTime(2016, 3, 23),
					Published = new DateTime(2016, 4, 13),
					PartedThingId_CreatorUri = "sovelto.fi/inventory",
					PartedThingId_UniqueString = "T2",
				});
				dbc.SaveChanges();

				//ThingRoleMember

				dbc.SaveChanges();

				//ThingRelation
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id_CreatorUri = "sovelto.fi/inventory",
					Thing1_Id_UniqueString = "T0",
					Thing2_Id_CreatorUri = "example.fi/inventory",
					Thing2_Id_UniqueString = "T1",
					RelationId = (int)RelationEnum.Belongings
				});
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id_CreatorUri = "sovelto.fi/inventory",
					Thing1_Id_UniqueString = "T0",
					Thing2_Id_CreatorUri = "example.fi/inventory",
					Thing2_Id_UniqueString = "T1",
					RelationId = (int)RelationEnum.RoleIn
				});
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id_CreatorUri = "sovelto.fi/inventory",
					Thing1_Id_UniqueString = "T1",
					Thing2_Id_CreatorUri = "example.fi/inventory",
					Thing2_Id_UniqueString = "T2",
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
					Console.WriteLine($"  {item.Id_UniqueString}");
				}

				Console.WriteLine("\nArchetypeThings, version 2");
				foreach (var item in dbc.Things.OfType<ArchetypeThing>())
				{
					Console.WriteLine($"  {item.Id_UniqueString}");
				}

				Console.WriteLine("\nEager Loading");
				foreach (var item in dbc.Things.Include(e => e.ThingRelations).ThenInclude(e => e.Relation))
				{
					Console.WriteLine($"  {item.Id_UniqueString}");
					foreach (var tr in item.ThingRelations)
					{
						Console.WriteLine($"      Relation to: {tr.Thing2_Id_CreatorUri}/{tr.Thing2_Id_UniqueString} Relation:{tr.Relation}");
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
