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
		public List<string> Services { get; set; }
	}

}
