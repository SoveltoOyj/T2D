using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;

namespace T2D.Infra.TestData
{
	public static class CommonTestData
	{
		private static int _next = 1;  //id 1 is reserved for anonymous user
		public static string Fqdn = "inv1.sovelto.fi";

		public static Dictionary<string, IEntity> Entities = new Dictionary<string, IEntity>();

		public static Guid Next()
		{
			_next++;
			return GetGuid(_next);
		}

		public static Guid GetGuid(int i)
		{
			var bytes = BitConverter.GetBytes(i);
			return new Guid(0, 0, 0, 0, 0, 0, 0, bytes[3], bytes[2], bytes[1], bytes[0]);
		}



		public static BaseThing FindByThingId(EfContext dbc, string fqdn, string uniqueString)
		{
			return dbc.Things.FirstOrDefault(t => t.Fqdn == fqdn && t.US == uniqueString);
		}


		public static void AddEnumData<TEntity>(EfContext dbc, DbSet<TEntity> dbSet, Type enumType)
		where TEntity : class, IEnumEntity, new()
		{
			foreach (var item in Enum.GetNames(enumType))
			{
				dbSet.Add(new TEntity { Id = (int)Enum.Parse(enumType, item, false), Name = item });
			}
			SaveIdentityOnData<TEntity>(dbc);
		}

		public static void SaveIdentityOnData<TEntity>(EfContext dbc)
			where TEntity : class
		{
			//T2D.Infra.EfContext dbc = ((IInfrastructure<IServiceProvider>)dbSet).GetService<DbContext>() as T2D.Infra.EfContext;

			var entityType = dbc.Model.FindEntityType(typeof(TEntity));
			var table = entityType.SqlServer();
			string tableName;
			if (string.IsNullOrWhiteSpace(table.Schema))
				tableName = table.TableName;
			else
				tableName = table.Schema + "." + table.TableName;

			dbc.Database.OpenConnection();
			try
			{
				dbc.Database.ExecuteSqlCommand($"Set identity_insert {tableName} on;");
				dbc.SaveChanges();
			}
			finally
			{
				dbc.Database.ExecuteSqlCommand($"Set identity_insert {tableName} off;");
			}
		}

	}
}
