using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace T2D.Entities
{
	/// <summary>
	/// Things that are used for authentication.
	/// </summary>
	public class AuthenticationThing : BaseThing, IInventoryThing
	{
		[StringLength(1024)]
		public string Title { get; set; }

		/// <summary>
		/// This relation is done using "Timpan tapa", not by using relations.
		/// </summary>
		public Guid? PersonThingId { get; set; }

		#region Navigation Properties
		public AuthenticationThing PersonThing { get; set; }
		public List<Session> Sessions { get; set; }
		#endregion

		public AuthenticationThing()
		{
			Sessions = new List<Session>();
		}

	}
}
