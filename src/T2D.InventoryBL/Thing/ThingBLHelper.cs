using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using T2D.Entities;
using T2D.Infra;
using T2D.Model.Helpers;

namespace T2D.InventoryBL
{
	public static class ThingBLHelper
	{
		public static T FindThing<T>(this EfContext dbc, string thingId)
			where T : class, T2D.Entities.IThing
		{
			return dbc.Things.SingleOrDefault(t => t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US == ThingIdHelper.GetUniqueString(thingId)) as T;
		}
		public static T FindThing<T>(this EfContext dbc, Guid id)
			where T : class, T2D.Entities.IThing
		{
			return dbc.Things.Find(id) as T;
		}

		public static IQueryable<BaseThing> ThingQuery(this EfContext dbc, string thingId)
		{
			return dbc.Things
				.Where(t => t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US == ThingIdHelper.GetUniqueString(thingId))
				.AsQueryable()
				;
		}

		public static IQueryable<TThing> ThingQuery<TThing>(this EfContext dbc, string thingId)
			where TThing : class, T2D.Entities.IThing
		{
			return dbc.Things
				.OfType<TThing>()
				.Where(t => t.Fqdn == ThingIdHelper.GetFQDN(thingId) && t.US == ThingIdHelper.GetUniqueString(thingId))
				.AsQueryable<TThing>()
				;
		}

		public static string GetThingStrId(this EfContext dbc, IThing thing)
		{
			return ThingIdHelper.Create(thing.Fqdn, thing.US);
		}


	}
}
