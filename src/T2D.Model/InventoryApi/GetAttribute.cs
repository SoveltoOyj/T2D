using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{
	public class GetAttributeRequest : BaseRequest
	{
		public string Attribute { get; set; }
	}

	public class GetAttributeResponse
	{
		public string Attribute { get; set; }
		public DateTime TimeStamp { get; set; }
		public object	Value { get; set; }
	}
}
