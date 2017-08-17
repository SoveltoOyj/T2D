using System;
using System.Collections.Generic;
using System.Text;
using T2D.Model.InventoryApi;

namespace T2D.Model.ServiceApi
{
	public class GetServiceStatusRequest : BaseRequest
	{
		/// <summary>
		/// Service ID (Guid). If null, returns last 10 Service Request statuses
		/// </summary>
		public Guid? ServiceId { get; set; }
	}
	public class GetServiceStatusResponse
	{
		public List<ServiceStatusResponse> Statuses { get; set; }
	}

	public class ServiceStatusResponse
	{
		public Guid ServiceId { get; set; }
		public string Title { get; set; }
		public DateTime RequestedAt { get; set; }
		public string State { get; set; }
		public DateTime? DeadLine { get; set; }
	}
}
