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
			}
			finally
			{
				dbc.Database.CloseConnection();
			}
			Console.WriteLine("\nDone");
			Console.ReadLine();
		}
	}

}
