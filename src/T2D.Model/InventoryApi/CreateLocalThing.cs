using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Query my roles request
	/// </summary>
	public class CreateLocalThingRequest
	{
		/// <summary>
		/// Session ID
		/// </summary>
		public string Session { get; set; }
		/// <summary>
		/// Thing ID of the new Thing: CreatorFQDN/UniqueString
		/// </summary>
		public string ThingId { get; set; }

		public string Title { get; set; }

		public ThingType ThingType { get; set; }

		/// <summary>
		/// The Thing that has omnipotent role to this new Thing.
		/// </summary>
		public string OmnipotentThingId { get; set; }
	}
}
