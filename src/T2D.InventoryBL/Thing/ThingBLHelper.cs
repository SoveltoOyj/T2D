using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
	}
}
