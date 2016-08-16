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
			Console.WriteLine("Create new T2D database, press Y");
			var ki = Console.ReadKey();
			if (ki.Key.ToString().ToLower() != "y") return;

			Console.WriteLine("\nCreating data.....");
			var dbc = new EfContext();
			//create database, add base data

			//relations
			dbc.Database.ExecuteSqlCommand("Set identity_insert relations on;");
			dbc.SaveChanges();
			foreach(var item in Enum.GetNames(typeof(RelationEnum)))
			{
				dbc.Relations.Add(new Relation { Id = (int) Enum.Parse(typeof(RelationEnum), item,false), Name = item });
			}
			dbc.SaveChanges();
			dbc.Database.ExecuteSqlCommand("Set identity_insert relations off");

			return;

			//roles
			foreach (var item in Enum.GetNames(typeof(RoleEnum)))
			{
				dbc.Roles.Add(new Role { Id = (int)Enum.Parse(typeof(RoleEnum), item, false), Name = item });
			}

			dbc.SaveChanges();
			Console.WriteLine("\nDone");
			Console.ReadLine();
		}
	}

}
