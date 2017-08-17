using System;
using System.Collections.Generic;
using System.Text;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
    public class UpdateActionStatusRequest:BaseRequest
    {
		public Guid ActionId { get; set; }
		public string State { get; set; }
	}
}
