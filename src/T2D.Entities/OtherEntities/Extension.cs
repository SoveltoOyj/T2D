using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using T2D.Helpers;

namespace T2D.Entities
{
	/// <summary>
	/// Extension 'definitions'.
	/// Implements IThing.
	/// </summary>
	public class Extension: IThing
	{
		public Guid Id { get; set; }

		[StringLength(256), Required]
		public string Fqdn { get; set; }

		[StringLength(512), Required]
		public string US { get; set; }

		public override string ToString()
		{
			return this.ToJson();
		}
	}
}
