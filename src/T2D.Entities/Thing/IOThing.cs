using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// IoT Thing.
	/// </summary>
	public class IoThing : BaseThing, IInventoryThing
	{
		[StringLength(1024)]
		public string Title { get; set; }
	}
}
