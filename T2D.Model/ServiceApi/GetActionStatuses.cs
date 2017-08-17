using System;
using System.Collections.Generic;
using System.Text;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
	public class GetActionStatusesRequest : BaseRequest
	{
	}

	public class GetActionStatusesResponse
	{
		public List<ActionStatusResponse> Statuses { get; set; }
	}

	public class ActionStatusResponse
	{
		public Guid ActionId { get; set; }
		public string Title { get; set; }
		public DateTime AddedAt { get; set; }
		public string State { get; set; }
		public string ActionType { get; set; }
		public string ActionClass { get; set; }
	}
}
