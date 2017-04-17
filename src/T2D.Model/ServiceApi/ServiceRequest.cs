using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
	public class ServiceRequestRequest : BaseRequest
	{
		/// <summary>
		/// Title of the Service that will be activated
		/// </summary>
		[Required]
		public string Service { get; set; }
	}

	//public class ServiceRequestResponse
	//{
	//	public bool Ok { get; set; }
	//}

}
