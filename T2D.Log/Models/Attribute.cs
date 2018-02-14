using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace T2D.Log.Models
{
	public class AttributeLogItem
	{
		[JsonProperty(PropertyName = "id")]
		public Guid Id { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "isComplete")]
		public bool Completed { get; set; }
	}

}
