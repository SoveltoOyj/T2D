using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Query my roles request
	/// </summary>
	public class CreateLocalThingRequest:BaseRequest
	{
		/// <summary>
		/// Thing ID of the new Thing: CreatorFQDN/UniqueString
		/// </summary>
		[Required]
		public string NewThingId { get; set; }

		public string Title { get; set; }
		[Required]
		public ThingType ThingType { get; set; }

	}
}
