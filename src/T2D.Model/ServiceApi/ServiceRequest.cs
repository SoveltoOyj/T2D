using System;
using System.Collections.Generic;
using System.Text;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
	public class ServiceRequestRequest : BaseRequest
	{
		public string Service { get; set; }
	}

	//public class ServiceRequestResponse
	//{
	//	public bool Ok { get; set; }
	//}

}
