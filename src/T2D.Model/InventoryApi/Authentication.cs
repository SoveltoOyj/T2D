using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using T2D.Model.Enums;

namespace T2D.Model.InventoryApi
{
	/// <summary>
	/// Authentication request
	/// </summary>
	public class AuthenticationRequest
	{
		/// <summary>
		/// The authenticated thing that will be the Session Thing.
		/// </summary>
		[Required]
		public string ThingId { get; set; }

		/// <summary>
		/// Authentication type, enum value.
		/// </summary>
		[Required]
		public AuthenticationType AuthenticationType { get; set; }
	}

	/// <summary>
	/// Returns Session Id.
	/// Note. Session secret will be added, not implemented yet.
	/// </summary>
	public class AuthenticationResponse
	{
		public string Session { get; set; }
	}

	
}
