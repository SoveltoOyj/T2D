using System;
using System.Collections.Generic;
using System.Text;
using static T2D.Model.InventoryApi.GetRelationsResponse.RelationsThings;

namespace T2D.Model.InventoryApi
{
    public class GetNearbyPublicLocationThingsRequest
    {
		/// <summary>
		/// Location from where nearby Things are searched.
		/// </summary>
			public GpsLocation GpsLocation { get; set; }

		/// <summary>
		/// Maximun distance in meters.
		/// </summary>
		public double Distance { get; set; }

		/// <summary>
		/// Which page, first page is 0.
		/// </summary>
		public int CurrentPage { get; set; }

		/// <summary>
		/// PageSize. If missing (or 0) then first 50 things are returned.
		/// </summary>
		public int PageSize { get; set; }
	}

	public class GetNearbyPublicLocationThingsResponse
	{
		public List<IdTitleDistance> Things { get; set; }

	}
	public class IdTitleDistance
	{
		public string ThingId { get; set; }
		public string Title { get; set; }

		public double Distance{ get; set; }
	}


}
