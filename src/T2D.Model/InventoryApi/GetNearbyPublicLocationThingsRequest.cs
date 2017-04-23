using System;
using System.Collections.Generic;
using System.Text;
using static T2D.Model.InventoryApi.GetRelationsResponse.RelationsThings;

namespace T2D.Model.InventoryApi
{
    public class GetNearbyPublicLocationThingsRequest
    {
		/// <summary>
		/// Location from where query is done.
		/// </summary>
			public GpsLocation GpsLocation { get; set; }

		/// <summary>
		/// Maximun distance in meters.
		/// </summary>
		public decimal Distance { get; set; }
	}

	public class GetNearbyPublicLocationThingsResponse
	{
		public List<IdTitleDistance> Things { get; set; }

	}
	public class IdTitleDistance
	{
		public IdTitle IdTitle { get; set; }
		public decimal Distance{ get; set; }
	}


}
