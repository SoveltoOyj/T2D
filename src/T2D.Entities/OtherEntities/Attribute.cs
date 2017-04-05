using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Attribute and Extension metadata
	/// </summary>
	public class Attribute : IEntity
	{
		public Guid Id { get; set; }
		[StringLength(768)]
		public string Title { get; set; }

		public AttributeType AttributeType { get; set; }

		[StringLength(1024)]
		public string Pattern { get; set; }
		[StringLength(256)]
		public string MinValue { get; set; }
		[StringLength(256)]
		public string MaxValue { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
