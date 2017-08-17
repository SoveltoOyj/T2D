using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// Wallet Thing.
	/// </summary>
	public class WalletThing : BaseThing, IInventoryThing
	{
		[StringLength(1024)]
		public string Title { get; set; }
	}
}
