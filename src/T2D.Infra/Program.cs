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
			var dbc = new EfContext();
			bool doLoop = true;
			while (doLoop)
			{
				Console.WriteLine("");
				Console.WriteLine("1 = Create new T2D database (existing db will be deleted)");
				Console.WriteLine("2 = Create new T2D database (existing db will be deleted) and insert base data");
				Console.WriteLine("3 = Insert Base data");
				Console.WriteLine("4 = Insert extra data 1");
				Console.WriteLine("5 = Insert ServiceRequest data");
				Console.WriteLine("Other key => cancel");
				var ki = Console.ReadKey();

				int key = 0;
				int.TryParse(ki.KeyChar.ToString(), out key);
				switch (key)
				{
					case 1:
						CreateDB(dbc);
						break;
					case 2:
						CreateDB(dbc);
					 	new TestData.BasicData(dbc).DoIt();
						PrintData();
						break;
					case 3:
						new TestData.BasicData(dbc).DoIt();
						PrintData();
						break;
					case 4:
						InsertExtraData(dbc);
						break;
					case 5:
						new TestData.Service_Action_TestData(dbc).DoIt();
						break;
					default:
						doLoop = false;
						break;
				}
			}
			return;
		}

		private static void CreateDB(EfContext dbc)
		{
			Console.WriteLine("\nCreating database ...");
			dbc.Database.EnsureDeleted();
			dbc.Database.EnsureCreated();
		}

		private static void InsertExtraData(EfContext dbc)
		{
			Console.WriteLine("\nCreating Extra data ...");
			//MyCar
			//MyTV
			string fqdn = "inv1.sovelto.fi";
			//AuthenticationThings
			var M100 = TestData.CommonTestData.FindByThingId(dbc, fqdn, "M100");
			if (M100 == null)
				throw new Exception("Can't find M100");

			//things
			dbc.RegularThings.Add(new RegularThing
			{
				Fqdn = fqdn,
				US = "S1",
				Title = "MyCar",
				Created = new DateTime(2015, 3, 1),
				IsLocalOnly = true,
				StatusId = 1,
				LocationTypeId = 1,
				Logging = true,
				Preferred_LocationTypeId = 1,
				Modified = new DateTime(2016, 3, 23),
				Published = new DateTime(2016, 4, 13),
				Creator_Fqdn = M100.Fqdn,
				Creator_US = M100.US
			});
			dbc.SaveChanges();
			var S1 = TestData.CommonTestData.FindByThingId(dbc, fqdn, "S1");

			dbc.RegularThings.Add(new RegularThing
			{
				Fqdn = fqdn,
				US = "S2",
				Title = "MyTV",
				Created = new DateTime(2015, 3, 1),
				IsLocalOnly = true,
				StatusId = 1,
				LocationTypeId = 2,
				Location_Gps = "(123.0, 55.9)",
				Logging = true,
				Preferred_LocationTypeId = 1,
				Modified = new DateTime(2014, 3, 3),
				Published = new DateTime(2012, 4, 13)
			});
			dbc.SaveChanges();
			var S2 = TestData.CommonTestData.FindByThingId(dbc, fqdn, "S2");


			//ThingRoleMember

			//add owner role to S1 and S2 for M100
			foreach (var thing in new BaseThing[] { S1, S2 })
			{
				var tr = new ThingRole { RoleId = (int)RoleEnum.Owner, ThingId = thing.Id };
				dbc.ThingRoles.Add(tr);
				var trm = new ThingRoleMember { ThingId = M100.Id, ThingRoleId = tr.Id };
				dbc.ThingRoleMembers.Add(trm);
				dbc.SaveChanges();
			}

			//add relation to S1 and S2 from M100
			foreach (var thing in new BaseThing[] { S1, S2 })
			{
				dbc.ThingRelations.Add(new ThingRelation
				{
					Thing1_Id = M100.Id,
					Thing2_Fqdn = thing.Fqdn,
					Thing2_US = thing.US,
					RelationId = (int)RelationEnum.Belongings
				});
				dbc.SaveChanges();
			}
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

	
		private static void AddAttributeData(EfContext dbc, Type enumType)
		{
			var entityType = dbc.Model.FindEntityType(typeof(Entities.Attribute));
			var table = entityType.SqlServer();
			var tableName = table.Schema + "." + table.TableName;

			foreach (var item in Enum.GetNames(enumType))
			{
				//ToDo: Add min/max/Pattern and so on...
				dbc.Attributes.Add(new Entities.Attribute { Id = (int)Enum.Parse(enumType, item, false), Name = item });
			}
			TestData.CommonTestData.SaveIdentityOnData<Entities.Attribute>(dbc);
		}

		
	}
}
