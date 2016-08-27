using Microsoft.EntityFrameworkCore;
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
				//relations
				dbc.Database.ExecuteSqlCommand("Set identity_insert relations on;");
				foreach (var item in Enum.GetNames(typeof(RelationEnum)))
				{
					dbc.Relations.Add(new Relation { Id = (int)Enum.Parse(typeof(RelationEnum), item, false), Name = item });
				}
				dbc.SaveChanges();
				dbc.Database.ExecuteSqlCommand("Set identity_insert relations off");

				//roles
				dbc.Database.ExecuteSqlCommand("Set identity_insert Roles on;");
				foreach (var item in Enum.GetNames(typeof(RoleEnum)))
				{
					dbc.Roles.Add(new Role { Id = (int)Enum.Parse(typeof(RoleEnum), item, false), Name = item });
				}
				dbc.SaveChanges();
				dbc.Database.ExecuteSqlCommand("Set identity_insert Roles off;");

				//Archetypthings
				dbc.ArchetypeThings.Add(new ArchetypeThing { Id_CreatorUri = "sovelto.fi/inventory", Id_UniqueString = "ArcNb1", Modified = new DateTime(2016, 3, 23), Published = new DateTime(2016, 4, 13) });
				//things
				dbc.RegularThings.Add(new RegularThing { Id_CreatorUri = "sovelto.fi/inventory", Id_UniqueString = "ThingNb1", Heightmm = 123000, Widthmm = 432000, Modified = new DateTime(2016, 3, 23), Published = new DateTime(2016, 4, 13) });
				dbc.RegularThings.Add(new RegularThing { Id_CreatorUri = "sovelto.fi/inventory", Id_UniqueString = "ThingNb2", Heightmm = 124500, Widthmm = 43000, Modified = new DateTime(2015, 8, 23), Published = new DateTime(2015, 4, 8) });
				dbc.RegularThings.Add(new RegularThing { Id_CreatorUri = "sovelto.fi/inventory", Id_UniqueString = "ThingNb3", Heightmm = 123000, Widthmm = 4322000, Modified = new DateTime(2016, 1, 23), Published = new DateTime(2016, 2, 4), ArchetypeThingId_CreatorUri="sovelto.fi/inventory", ArchetypeThingId_UniqueString= "ArcNb1" });
				dbc.SaveChanges();

				//ThingRoleMember
				dbc.ThingRoleMembers.Add(new ThingRoleMember { Member_ThingId_CreatorUri = "sovelto.fi/inventory", Member_ThingId_UniqueString = "ThingNb1", ThingId_CreatorUri = "sovelto.fi/inventory", ThingId_UniqueString = "ThingNb2", ThingRoleId = 1 });
				dbc.SaveChanges();

				//ThingRelation
				dbc.ThingRelations.Add(new ThingRelation { Thing1_Id_CreatorUri = "sovelto.fi/inventory", Thing1_Id_UniqueString = "ThingNb1", Thing2_Id_CreatorUri = "example.fi/inventory", Thing2_Id_UniqueString = "123", RelationId = 1 });
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
				foreach (var item in dbc.Things.Include(e=>e.ThingRelations).ThenInclude(e=>e.Relation))
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
	}
}
