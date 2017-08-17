using System;
using System.Collections.Generic;
using System.Text;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
	public class GetServicesRequest : BaseRequest
	{
	}

	public class GetServicesResponse
	{
		/// <summary>
		/// Titles of Services this Thing has.
		/// </summary>
		public List<string> Services { get; set; }
	}

}
