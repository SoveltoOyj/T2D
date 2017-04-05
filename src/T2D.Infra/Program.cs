using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Entities;
using T2D.Infra.TestData;

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
				Console.WriteLine("4 = Print Data");
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
						PrintData();
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

				var q = dbc.Things
									.Include(e => e.ToThingRelations)
									   .ThenInclude(e => e.Relation)
									.Include(e=>e.ToThingRelations)
									   .ThenInclude(e => e.ToThing)
									;

				Console.WriteLine("\nEager Loading");
				foreach (var item in q)
				{
					Console.WriteLine($"  {item.US}");
					foreach (var tr in item.ToThingRelations)
					{
						Console.WriteLine($"      Relation to: {tr.ToThing.Fqdn}/{tr.ToThing.US} Relation:{tr.Relation}");
					}
					Console.WriteLine();
				}
			}
		}


		
	}
}
