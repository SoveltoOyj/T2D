using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Model.InventoryApi
{
	public class GetAttributesRequest : BaseRequest
	{
		/// <summary>
		/// List of attributes which values to read.
		/// </summary>
		public List<string> Attributes { get; set; }
	}

	public class GetAttributesResponse
	{
		public List<AttributeValue> AttributeValues { get; set; }
	}

	public class AttributeValue
	{
		/// <summary>
		/// Attribute
		/// </summary>
		public string Attribute { get; set; }

		/// <summary>
		/// Could attribute value be read.
		/// </summary>
		public bool IsOk { get; set; }

		/// <summary>
		/// The reason for not be able to read (if IsOk == false).
		/// </summary>
		public string ErrorDescription { get; set; }

		/// <summary>
		/// Last updateTime for this attribute.
		/// </summary>
		public DateTime? TimeStamp { get; set; }

		/// <summary>
		/// Value of the attribute
		/// </summary>
		public object Value { get; set; }
	}

}
