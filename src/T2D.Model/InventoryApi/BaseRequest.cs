using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace T2D.Model.InventoryApi
{
	public abstract class BaseRequest
	{
		/// <summary>
		/// Session that will be used in this request
		/// </summary>
		[Required]
		public string Session { get; set; }
		/// <summary>
		/// ThingID that will be used in this request
		/// </summary>
		[Required]
		public string ThingId { get; set; }

		/// <summary>
		/// This request will be done in this role
		/// </summary>
		[Required]
		public string Role { get; set; }
	}

}
